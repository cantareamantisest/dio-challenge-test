using Desafio.Dio.Core.Entities;
using Desafio.Dio.Core.Interfaces;
using Desafio.Dio.Tests.Fixtures;
using Moq;

namespace Desafio.Dio.Tests
{
    [Collection(nameof(CategoryColletion))]
    public class CategoryServiceTests
    {
        private readonly CategoryFixture _fixture;

        public CategoryServiceTests(CategoryFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Obter Categorias")]
        public void CategoryService_GetAll_ReturnsAnyCategory()
        {
            // Arrange
            int count = 10;
            var categoryService = _fixture.GetCategoryService();
            _fixture.Mocker.GetMock<ICategoryRepository>().Setup(c => c.GetAll()).Returns(_fixture.GetAllCategories(count));

            // Act
            var categories = categoryService.GetAll();

            // Assert
            _fixture.Mocker.GetMock<ICategoryRepository>().Verify(r => r.GetAll(), Times.Once());
            Assert.True(categories.Any());
            Assert.Equal(count, categories.Count());
        }

        [Fact(DisplayName = "Adicionar Categoria")]
        public void CategoryService_AddCategory_ReturnsCreated()
        {
            // Arrange
            var categoryService = _fixture.GetCategoryService();
            var newCategory = _fixture.GetCategory();
            _fixture.Mocker.GetMock<ICategoryRepository>().Setup(c => c.Add(newCategory));
            _fixture.Mocker.GetMock<ICategoryRepository>().Setup(c => c.GetById(newCategory.Id)).Returns(newCategory);

            // Act
            categoryService.Add(newCategory);
            var addedCategory = categoryService.GetById(newCategory.Id);

            // Assert
            _fixture.Mocker.GetMock<ICategoryRepository>().Verify(c => c.Add(newCategory), Times.Once());
            _fixture.Mocker.GetMock<ICategoryRepository>().Verify(c => c.Add(It.Is<Category>(p =>
            p.Id == newCategory.Id &&
            p.Name == newCategory.Name)), Times.Once);
            Assert.Equal(newCategory, addedCategory);
        }

        [Fact(DisplayName = "Atualizar Categoria")]
        public void CategoryService_UpdateCategory_VerifyUpdate()
        {
            // Arrange
            var categoryService = _fixture.GetCategoryService();
            var category = _fixture.GetCategory();
            string categoryName = category.Name + " updated";
            category.Name = categoryName;
            var updatedCategory = category;
            _fixture.Mocker.GetMock<ICategoryRepository>().Setup(c => c.Update(category));

            // Act
            categoryService.Update(category);

            // Assert
            _fixture.Mocker.GetMock<ICategoryRepository>().Verify(c => c.Update(category), Times.Once());
            _fixture.Mocker.GetMock<ICategoryRepository>().Verify(c => c.Update(It.Is<Category>(p =>
            p.Id == category.Id &&
            p.Name == categoryName)), Times.Once);
        }

        [Fact(DisplayName = "Remover Categoria")]
        public void CategoryService_RemoveCategory_VerifyDelete()
        {
            // Arrange
            var categoryService = _fixture.GetCategoryService();
            var category = _fixture.GetCategory();
            _fixture.Mocker.GetMock<ICategoryRepository>().Setup(c => c.Delete(category.Id));

            // Act
            categoryService.Delete(category.Id);

            // Assert
            _fixture.Mocker.GetMock<ICategoryRepository>().Verify(c => c.Delete(category.Id), Times.Once());
            _fixture.Mocker.GetMock<ICategoryRepository>().Verify(c => c.Delete(It.Is<int>(p => p == category.Id)), Times.Once);
        }
    }
}