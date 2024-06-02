using Desafio.Dio.Application.Interfaces;
using Desafio.Dio.Core.Entities;
using Desafio.Dio.Core.Interfaces;

namespace Desafio.Dio.Application.Services
{
    public class CategoryService : ServiceBase<Category>, ICategoryService
    {
        private readonly ICategoryRepository _repository;

        public CategoryService(ICategoryRepository repository)
            : base(repository)
        {
            _repository = repository;
        }
    }
}