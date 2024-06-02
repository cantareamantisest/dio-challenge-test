using Bogus;
using Desafio.Dio.Application.Services;
using Desafio.Dio.Core.Entities;
using Moq.AutoMock;

namespace Desafio.Dio.Tests.Fixtures
{
    [CollectionDefinition(nameof(CategoryColletion))]
    public class CategoryColletion : ICollectionFixture<CategoryFixture> { }

    public class CategoryFixture : IDisposable
    {
        public CategoryService CategoryService { get; private set; }
        public AutoMocker Mocker { get; private set; }
        private Faker<Category> _categoryFaker;

        public IEnumerable<Category> GetAllCategories(int count)
        {
            _categoryFaker = new Faker<Category>()
                .RuleFor(c => c.Id, f => f.IndexFaker + 1)
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

        public void Dispose()
        {
        }
    }
}