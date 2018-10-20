using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PimpMyRide.Core.Data.Context;

namespace PimpMyRide.Core.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly PimpMyRideDbContext _context;

        public Repository(PimpMyRideDbContext context)
        {
            _context = context;
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>();
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate).AsQueryable();
        }

        public T GetById(object id)
        {
            return _context.Set<T>().Find(id);
        }

        public void Add(T entity)
        {
            if (entity != null)
            {
                _context.Set<T>().Add(entity);
            }
        }

        public async Task AddAsync(T entity, CancellationToken cancellationToken)
        {
            if (entity != null)
            {
                await _context.Set<T>().AddAsync(entity, cancellationToken);
            }
        }

        public void Delete(T entity)
        {
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
            }
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.Set<T>().Attach(entity);
        }
    }
}
