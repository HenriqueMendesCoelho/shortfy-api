using suavesabor_api.Domain.Base;

namespace suavesabor_api.Repository.Generic
{
    public interface IGenericRepository<T, Tkey> where T : IEntity<Tkey>
    {
        T? Create(T item);

        T? Update(T item);

        void Delete(T id);

        T? FindByID(T id);

        List<T> FindAll();

        Boolean Exists(T item);
    }
}
