using Desafio.Dio.Identity.Models;
using Desafio.Dio.Integration.Tests.Config;
using Desafio.Dio.Integration.Tests.Fixtures;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Desafio.Dio.Integration.Tests
{
    [Collection(nameof(IntegrationApiTestsFixtureCollection))]
    public class CategoryControllerTests : AppFactory<Program>
    {
        private readonly IntegrationTestsFixture<Program> _fixture;

        public CategoryControllerTests(IntegrationTestsFixture<Program> fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName ="Adicionar Categoria Web API")]
        public async Task Add_Category_ReturnsStatusCodeCreated()
        {
            // Arrange
            await _fixture.RegisterUserAndLogin();
            _fixture.HttpClient.SetToken(_fixture.UserToken);
            var category = _fixture.GetCategory();

            // Act
            var postResponse = await _fixture.HttpClient.PostAsJsonAsync("api/v1/category", category);

            // Assert
            postResponse.EnsureSuccessStatusCode();
        }

        [Fact(DisplayName = "Obter todas Categorias - Web API")]
        public async Task GetAll_Categories_ShouldReturnsOK()
        {
            // Arrange
            await _fixture.RegisterUserAndLogin();
            _fixture.HttpClient.SetToken(_fixture.UserToken);

            // Act
            var response = await _fixture.HttpClient.GetAsync("api/v1/category");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact(DisplayName = "Obter Categoria por Id - Web API")]
        public async Task GetById_Category_ShouldReturnsOK()
        {
            // Arrange
            await _fixture.RegisterUserAndLogin();
            _fixture.HttpClient.SetToken(_fixture.UserToken);

            var category = _fixture.GetCategory();

            // Act
            var postResponse = await _fixture.HttpClient.PostAsJsonAsync("api/v1/category", category);
            postResponse.EnsureSuccessStatusCode();
            var responseString = await postResponse.Content.ReadAsStringAsync(CancellationToken.None);
            var data = (JObject)JsonConvert.DeserializeObject(@responseString);
            var id = data["id"].Value<int>();
            var responseGetById = await _fixture.HttpClient.GetAsync($"api/v1/category/{id}");

            // Assert
            responseGetById.EnsureSuccessStatusCode();
            var responseStringGetById = await responseGetById.Content.ReadAsStringAsync(CancellationToken.None);

            Assert.Contains(category.Name, responseStringGetById);
        }
    }
}
