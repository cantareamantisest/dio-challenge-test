using Desafio.Dio.Application.Interfaces;
using Desafio.Dio.Core.Interfaces;

namespace Desafio.Dio.Application.Services
{
    public class ServiceBase<TEntity> : IServiceBase<TEntity> where TEntity : class
    {
        private readonly IRepositoryBase<TEntity> _repository;

        public ServiceBase(IRepositoryBase<TEntity> repository)
        {
            this._repository = repository;
        }

        public TEntity Add(TEntity obj)
        {
            return this._repository.Add(obj);
        }

        public void Delete(int id)
        {
            this._repository.Delete(id);
        }

        public void Update(TEntity obj)
        {
            this._repository.Update(obj);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return this._repository.GetAll();
        }

        public TEntity GetById(int id)
        {
            return this._repository.GetById(id);
        }
    }
}