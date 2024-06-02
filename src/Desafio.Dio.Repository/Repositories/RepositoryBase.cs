using Desafio.Dio.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Desafio.Dio.Repository.Repositories
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        protected readonly DbContext _dbContext;
        protected readonly DbSet<TEntity> _dbSet;

        public RepositoryBase(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentException(nameof(dbContext));
            _dbSet = _dbContext.Set<TEntity>();
        }

        public TEntity Add(TEntity obj)
        {
            var entityAdded = _dbSet.Add(obj);
            _dbContext.SaveChanges();
            return (TEntity) entityAdded.Entity;
        }

        public void Delete(int id)
        {
            TEntity obj = GetById(id);
            _dbSet.Remove(obj);
            _dbContext.SaveChanges();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _dbSet.ToList();
        }

        public TEntity GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public void Update(TEntity obj)
        {
            _dbSet.Update(obj);
            _dbContext.SaveChanges();
        }
    }
}