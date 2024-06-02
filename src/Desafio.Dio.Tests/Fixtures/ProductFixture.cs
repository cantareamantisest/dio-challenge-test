using Bogus;
using Desafio.Dio.Application.Services;
using Desafio.Dio.Core.Entities;
using Moq.AutoMock;

namespace Desafio.Dio.Tests.Fixtures
{
    [CollectionDefinition(nameof(ProductCollection))]
    public class ProductCollection : ICollectionFixture<ProductFixture> { }

    public class ProductFixture : IDisposable
    {
        public ProductService ProductService { get; private set; }
        public AutoMocker Mocker { get; private set; }
        private Faker<Product> _productFaker;
        private Faker<Category> _categoryFaker;

        public IEnumerable<Product> GetAllProducts(int count)
        {
            _categoryFaker = new Faker<Category>()
                .RuleFor(c => c.Id, f => f.IndexFaker + 1)
                .RuleFor(c => c.Name, f => f.Commerce.Department());

            var categories = _categoryFaker.Generate(count);

            _productFaker = new Faker<Product>()
                .RuleFor(c => c.Id, f => f.IndexFaker + 1)
                .RuleFor(c => c.Name, f => f.Commerce.ProductName())
                .RuleFor(c => c.Price, f => Math.Round(1000 * f.Random.Decimal(), 2))
                .RuleFor(c => c.Quantity, f => f.Random.Int())
                .RuleFor(c => c.IdCategory, f => f.PickRandom(categories).Id)
                .RuleFor(c => c.Category, f => f.PickRandom(categories));

            return _productFaker.Generate(count);
        }

        public Product GetProduct() => GetAllProducts(1).FirstOrDefault();

        public ProductService GetProductService()
        {
            Mocker = new AutoMocker();
            ProductService = Mocker.CreateInstance<ProductService>();
            return ProductService;
        }

        public void Dispose()
        {

        }
    }
}
