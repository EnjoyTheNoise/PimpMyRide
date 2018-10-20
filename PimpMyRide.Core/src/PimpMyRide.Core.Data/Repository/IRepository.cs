using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace PimpMyRide.Core.Data.Repository
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        IQueryable<T> Get(Expression<Func<T, bool>> predicate);
        T GetById(object id);
        void Add(T entity);
        Task AddAsync(T entity, CancellationToken cancellationToken);
        void Delete(T entity);
        void Update(T entity);
    }
}
