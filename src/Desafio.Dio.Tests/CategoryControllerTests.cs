using Microsoft.AspNetCore.Mvc.Testing;

namespace Desafio.Dio.Tests
{
    public class CategoryControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public CategoryControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        private async Task Example_Integration_Test()
        {
            //Arrange
            var httpClient = _factory.CreateClient();

            //Act
            var result = await httpClient.GetAsync("api/category");


            //Assert
            //Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            //var okResult = Assert.IsType<OkObjectResult>(result.Result);
            //var returnedProducts = Assert.IsType<List<Category>>(result.Content);
            //Assert.Equal(_fixture.FakeCategories.Count, returnedProducts.Count);
            //Assert.Equal(_fixture.FakeCategories, returnedProducts);
        }
    }
}
