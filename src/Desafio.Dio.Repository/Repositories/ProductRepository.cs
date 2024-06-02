using Desafio.Dio.Core.Entities;
using Desafio.Dio.Core.Interfaces;
using Desafio.Dio.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace Desafio.Dio.Repository.Repositories
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        private readonly DbSet<Product> _dbSetProduct;

        public ProductRepository(DesafioContext context)
            : base(context)
        {
            _dbSetProduct = context.Set<Product>();
        }
    }
}