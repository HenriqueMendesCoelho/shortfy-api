﻿using shortfy_api.src.Application.Domain.Base;

namespace shortfy_api.src.Application.Repository.Generic
{
    public interface IGenericRepository<T, TKey> where T : IEntity<TKey>
    {
        Task<T> Create(T Entity);

        Task<T?> Update(T Entity);

        Task<int> DeleteByID(TKey Id);

        Task<T?> FindByID(TKey Id);

        Task<List<T>> FindAll();

        Task<bool> Exists(T Entity);
    }
}
