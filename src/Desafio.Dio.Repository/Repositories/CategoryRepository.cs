using Desafio.Dio.Core.Entities;
using Desafio.Dio.Core.Interfaces;
using Desafio.Dio.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace Desafio.Dio.Repository.Repositories
{
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        private readonly DbSet<Category> _dbSetCategory;

        public CategoryRepository(DesafioContext context)
            : base(context)
        {
            _dbSetCategory = context.Set<Category>();
        }
    }
}