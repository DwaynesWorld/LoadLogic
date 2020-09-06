using LoadLogic.Services.Vendors.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoadLogic.Services.Abstractions;

namespace LoadLogic.Services.Vendors.Infrastructure.Persistence
{
    /// <inheritdoc />
    public class Repository<T> : ICrudRepository<T> where T : Entity, IAggregateRoot
    {
        private readonly VendorsContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public Repository(VendorsContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<T?> FindByIdAsync(long id, params string[] includes)
        {
            if (includes == null || includes.Length == 0)
            {
                return await _context.Set<T>().Where(x => x.Id == id).FirstOrDefaultAsync();
            }

            var query = _context.Set<T>().Where(x => x.Id == id);
            query = includes.Aggregate(query, (q, i) => q.Include(i));
            return await query.FirstOrDefaultAsync();
        }

        public async Task<T?> FindOneAsync(ISpecification<T> spec, params string[] includes)
        {
            if (includes == null || includes.Length == 0)
            {
                return await _context.Set<T>().Where(spec.SpecExpression).FirstOrDefaultAsync();
            }

            var query = _context.Set<T>().Where(spec.SpecExpression);
            query = includes.Aggregate(query, (q, i) => q.Include(i));
            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> FindAllAsync(ISpecification<T> spec, params string[] includes)
        {
            if (includes == null || includes.Length == 0)
            {
                return await _context.Set<T>().Where(spec.SpecExpression).ToListAsync();
            }

            var query = _context.Set<T>().Where(spec.SpecExpression);
            query = includes.Aggregate(query, (q, i) => q.Include(i));
            return await query.ToListAsync();
        }

        public async Task<bool> FindAnyAsync(ISpecification<T> spec, params string[] includes)
        {
            if (includes == null || includes.Length == 0)
            {
                return await _context.Set<T>().Where(spec.SpecExpression).AnyAsync();
            }

            var query = _context.Set<T>().Where(spec.SpecExpression);
            query = includes.Aggregate(query, (q, i) => q.Include(i));
            return await query.AnyAsync();
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

    }
}
