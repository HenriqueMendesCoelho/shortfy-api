using suavesabor_api.Application.Domain.Base;

namespace suavesabor_api.Application.Repository.Generic
{
    public interface IGenericRepository<T, TKey> where T : IEntity<TKey>
    {
        Task<T> Create(T Entity);

        Task<T?> Update(T Entity);

        void Delete(T Id);

        Task<T?> FindByID(TKey Id);

        Task<List<T>> FindAll();

        Task<bool> Exists(T Entity);
    }
}
