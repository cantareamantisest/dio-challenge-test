using Desafio.Dio.Identity.Models;
using Desafio.Dio.Integration.Tests.Config;
using Desafio.Dio.Integration.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Desafio.Dio.Integration.Tests
{
    [TestCaseOrderer("Desafio.Dio.Integration.Tests.Config.PriorityOrderer", "Desafio.Dio.Integration.Tests")]
    [Collection(nameof(IntegrationApiTestsFixtureCollection))]
    public class AccountControllerTests : AppFactory<Program>
    {
        private readonly IntegrationTestsFixture<Program> _fixture;

        public AccountControllerTests(IntegrationTestsFixture<Program> fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Register novo usuário válido"), TestPriority(1)]
        public async Task Register_ValidUser_ShouldReturnsOK()
        {
            // Arrange
            _fixture.GenerateUser();
            var userRegister = new RegisterRequest
            {
                FirstName = _fixture.FirstName,
                LastName = _fixture.LastName,
                Email = _fixture.UserEmail,
                Password = _fixture.UserPassword,
                ConfirmPassword = _fixture.UserPassword
            };

            // Act
            var postResponse = await _fixture.HttpClient.PostAsJsonAsync("api/v1/account/register", userRegister);

            //Assert
            postResponse.EnsureSuccessStatusCode();
        }

        [Fact(DisplayName = "Efetuar login com usuário válido"), TestPriority(2)]
        public async Task Login_ValidUser_ShouldReturnsOK()
        {
            // Arrange
            var userLogin = new LoginRequest 
            { 
                Email = _fixture.UserEmail, 
                Password = _fixture.UserPassword 
            };

            // Act
            var postResponse = await _fixture.HttpClient.PostAsJsonAsync("api/v1/account/login", userLogin);


            // Assert
            postResponse.EnsureSuccessStatusCode();
        }

    }
}
