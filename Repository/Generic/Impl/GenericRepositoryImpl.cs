using Microsoft.EntityFrameworkCore;
using suavesabor_api.Data;
using suavesabor_api.Domain.Base;
using suavesabor_api.Domain.User;

namespace suavesabor_api.Repository.Generic.Impl
{
    public class GenericRepositoryImpl<T, TKey> : IGenericRepository<T, TKey> where T : class, IEntity<TKey>
    {
        private readonly DataContext _context;
        public DbSet<T> Dataset { get; set; }
        public GenericRepositoryImpl(DataContext context)
        {
            _context = context;
            Dataset = _context.Set<T>();
        }

        public T? Create(T item)
        {
            if (item == null) 
            {
                return null;
            }

            Dataset.Add(item);
            _context.SaveChanges();

            return item;
        }

        public void Delete(T id)
        {
            if(id == null) { return; }

            var result = Dataset.SingleOrDefault(r => r.Id.Equals(id));
            if (result == null)
            {
                return;
            }

            Dataset.Remove(result);
            _context.SaveChanges();
        }

        public bool Exists(T item)
        {
            return Dataset.Any(i => i.Id.Equals(item.Id));
        }

        public List<T> FindAll()
        {
            return Dataset.ToList();
        }

        public T? FindByID(T id)
        {
            return Dataset.SingleOrDefault(r => r.Id.Equals(id));
        }

        public T? Update(T item)
        {
            var result = Dataset.SingleOrDefault(r => r.Id.Equals(item.Id));
            if (result == null)
            {
                return null;
            }


            _context.Entry(result).CurrentValues.SetValues(item);
            _context.SaveChanges();
            return result;
            
        }
    }
}
