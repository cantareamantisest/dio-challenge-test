namespace Desafio.Dio.Application.Interfaces
{
    public interface IServiceBase<TEntity> where TEntity : class
    {
        TEntity Add(TEntity obj);
        void Update(TEntity obj);
        void Delete(int id);
        IEnumerable<TEntity> GetAll();
        TEntity GetById(int id);
    }
}