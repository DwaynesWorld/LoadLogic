using LoadLogic.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoadLogic.Services.Vendors.Domain
{
    /// <summary>
    /// Responsible for context based database operations.
    /// Should be used when entity tracking is required.
    /// </summary>
    /// <typeparam name="T">The aggregate root enitity.</typeparam>
    public interface ICrudRepository<T> where T : class, IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
        Task<T?> FindByIdAsync(long id, params string[] includes);
        Task<T?> FindOneAsync(ISpecification<T> spec, params string[] includes);
        Task<IEnumerable<T>> FindAllAsync(ISpecification<T> spec, params string[] includes);
        Task<bool> FindAnyAsync(ISpecification<T> spec, params string[] includes);
        void Add(T entity);
        void Remove(T entity);
    }
}
