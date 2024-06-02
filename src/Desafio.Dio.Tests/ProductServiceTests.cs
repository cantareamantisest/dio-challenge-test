using Desafio.Dio.Core.Entities;
using Desafio.Dio.Core.Interfaces;
using Desafio.Dio.Tests.Fixtures;
using Moq;

namespace Desafio.Dio.Tests
{
    [Collection(nameof(ProductCollection))]
    public class ProductServiceTests
    {
        private readonly ProductFixture _fixture;

        public ProductServiceTests(ProductFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Obter Produtos")]
        public void ProductService_GetAll_ReturnsAnyProduct()
        {
            // Arrange
            int count = 10;
            var productService = _fixture.GetProductService();
            _fixture.Mocker.GetMock<IProductRepository>().Setup(c => c.GetAll()).Returns(_fixture.GetAllProducts(count));

            // Act
            var categories = productService.GetAll();

            // Assert
            _fixture.Mocker.GetMock<IProductRepository>().Verify(r => r.GetAll(), Times.Once());
            Assert.True(categories.Any());
            Assert.Equal(count, categories.Count());
        }

        [Fact(DisplayName = "Adicionar Produto")]
        public void ProductService_AddProduct_ReturnsCreated()
        {
            // Arrange
            var productService = _fixture.GetProductService();
            var newProduct = _fixture.GetProduct();
            _fixture.Mocker.GetMock<IProductRepository>().Setup(c => c.Add(newProduct));
            _fixture.Mocker.GetMock<IProductRepository>().Setup(c => c.GetById(newProduct.Id)).Returns(newProduct);

            // Act
            productService.Add(newProduct);
            var addedProduct = productService.GetById(newProduct.Id);

            // Assert
            _fixture.Mocker.GetMock<IProductRepository>().Verify(c => c.Add(newProduct), Times.Once());
            _fixture.Mocker.GetMock<IProductRepository>().Verify(c => c.Add(It.Is<Product>(p =>
            p.Id == newProduct.Id &&
            p.Name == newProduct.Name &&
            p.Price == newProduct.Price &&
            p.IdCategory == newProduct.IdCategory)), Times.Once);
            Assert.Equal(newProduct, addedProduct);
            Assert.NotNull(newProduct.Category);
        }

        [Fact(DisplayName = "Atualizar Produto")]
        public void ProductService_UpdateProduct_VerifyUpdate()
        {
            // Arrange
            var productService = _fixture.GetProductService();
            var product = _fixture.GetProduct();
            string productName = product.Name + " updated";
            product.Name = productName;
            var updatedProduct = product;
            _fixture.Mocker.GetMock<IProductRepository>().Setup(c => c.Update(product));

            // Act
            productService.Update(product);

            // Assert
            _fixture.Mocker.GetMock<IProductRepository>().Verify(c => c.Update(product), Times.Once());
            _fixture.Mocker.GetMock<IProductRepository>().Verify(c => c.Update(It.Is<Product>(p =>
            p.Id == product.Id &&
            p.Name == productName)), Times.Once);
            Assert.NotNull(product.Category);
        }

        [Fact(DisplayName = "Remover Produto")]
        public void ProductService_RemoveProduct_VerifyDelete()
        {
            // Arrange
            var productService = _fixture.GetProductService();
            var product = _fixture.GetProduct();
            _fixture.Mocker.GetMock<IProductRepository>().Setup(c => c.Delete(product.Id));

            // Act
            productService.Delete(product.Id);

            // Assert
            _fixture.Mocker.GetMock<IProductRepository>().Verify(c => c.Delete(product.Id), Times.Once());
            _fixture.Mocker.GetMock<IProductRepository>().Verify(c => c.Delete(It.Is<int>(p => p == product.Id)), Times.Once);
        }
    }
}
