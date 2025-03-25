using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Persistence.Dynamics.Concrete;
using Persistence.Paging.Abstract;
using Persistence.Repositories.Concrete;

namespace Persistence.Repositories.Abstract;

public interface IAsyncRepository<T> : IQuery<T> where T : Entity
{
    Task<T> GetAsync(Expression<Func<T, bool>> predicate);

    Task<IPaginate<T>> GetListAsync(Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, int index = 0, int size = 10,
        bool enableTracking = true, CancellationToken cancellationToken = default);

    Task<IPaginate<T>> GetListByDynamicAsync(Dynamic dynamic,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, int index = 0, int size = 10,
        bool enableTracking = true, CancellationToken cancellationToken = default);
    
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<T> DeleteAsync(T entity);
}