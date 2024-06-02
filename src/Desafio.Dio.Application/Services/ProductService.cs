using Desafio.Dio.Application.Interfaces;
using Desafio.Dio.Core.Entities;
using Desafio.Dio.Core.Interfaces;

namespace Desafio.Dio.Application.Services
{
    public class ProductService : ServiceBase<Product>, IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
            : base(repository)
        {
            _repository = repository;
        }
    }
}