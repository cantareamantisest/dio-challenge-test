using Bogus;
using Desafio.Dio.Application.Services;
using Desafio.Dio.Core.Entities;
using Desafio.Dio.Identity.Models;
using Desafio.Dio.Integration.Tests.Config;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq.AutoMock;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Json;
using System.Text.RegularExpressions;

namespace Desafio.Dio.Integration.Tests.Fixtures
{
    [CollectionDefinition(nameof(IntegrationApiTestsFixtureCollection))]
    public class IntegrationApiTestsFixtureCollection : ICollectionFixture<IntegrationTestsFixture<Program>> { }

    public class IntegrationTestsFixture<TProgram> : IDisposable where TProgram : class
    {
        public string AntiForgeryFieldName = "__RequestVerificationToken";
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string UserEmail { get; private set; }
        public string UserPassword { get; private set; }
        public string UserToken { get; private set; }
        public readonly AppFactory<TProgram> Factory;
        public HttpClient HttpClient { get; set; }
        public CategoryService CategoryService { get; private set; }
        public AutoMocker Mocker { get; private set; }
        private Faker<Category> _categoryFaker;

        public IntegrationTestsFixture()
        {
            var clientOptions = new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = true,
                HandleCookies = true,
                MaxAutomaticRedirections = 7
            };

            Factory = new AppFactory<TProgram>();
            HttpClient = Factory.CreateClient(clientOptions);
        }

        public IEnumerable<Category> GetAllCategories(int count)
        {
            _categoryFaker = new Faker<Category>()
                .RuleFor(c => c.Name, f => f.Commerce.Department());
            return _categoryFaker.Generate(count);
        }

        public Category GetCategory() => GetAllCategories(1).FirstOrDefault();

        public CategoryService GetCategoryService()
        {
            Mocker = new AutoMocker();
            CategoryService = Mocker.CreateInstance<CategoryService>();
            return CategoryService;
        }

        public void GenerateUser()
        {
            var faker = new Faker();
            FirstName = faker.Name.FirstName();
            LastName = faker.Name.LastName();
            UserEmail = faker.Internet.Email(FirstName, LastName, "fakemail.net").ToLower();
            UserPassword = faker.Internet.Password(12, false, "", "!0Wk");
        }

        public async Task RegisterUserAndLogin()
        {
            var faker = new Faker();
            FirstName = faker.Name.FirstName();
            LastName = faker.Name.LastName();
            UserEmail = faker.Internet.Email(FirstName, LastName, "fakemail.org").ToLower();
            UserPassword = faker.Internet.Password(10, 16);

            var registerData = new RegisterRequest
            {
                FirstName = FirstName,
                LastName = LastName,
                Email = UserEmail,
                Password = UserPassword,
                ConfirmPassword = UserPassword
            };

            var registerResponse = await HttpClient.PostAsJsonAsync("api/v1/account/register", registerData);
            registerResponse.EnsureSuccessStatusCode();
            var loginData = new LoginRequest
            {
                Email = UserEmail,
                Password = UserPassword
            };

            var loginResponse = await HttpClient.PostAsJsonAsync("api/v1/account/login", loginData);
            loginResponse.EnsureSuccessStatusCode();

            var responseString = await loginResponse.Content.ReadAsStringAsync(CancellationToken.None);

            var data = (JObject)JsonConvert.DeserializeObject(@responseString);
            var token = data["accessToken"].Value<string>();
            UserToken = token;
        }

        public string GetAntiForgeryToken(string htmlBody)
        {
            var requestVerificationTokenMatch = Regex.Match(htmlBody, $@"\<input name=""{AntiForgeryFieldName}"" type=""hidden"" value=""([^""]+)"" \/\>");
            if (requestVerificationTokenMatch.Success)
            {
                return requestVerificationTokenMatch.Groups[1].Captures[0].Value;
            }
            throw new ArgumentException($"Anti forgery token '{AntiForgeryFieldName}' not found in Html", htmlBody);
        }

        public void Dispose()
        {
            HttpClient?.Dispose();
            Factory?.Dispose();
        }
    }
}