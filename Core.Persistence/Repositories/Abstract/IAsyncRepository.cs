#region Namespace
/// <summary>
/// Contains abstract interfaces for asynchronous data repositories, including query and paginated methods.
/// </summary>
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Persistence.Dynamics.Concrete;
using Persistence.Paging.Abstract;
using Persistence.Repositories.Concrete;

namespace Persistence.Repositories.Abstract
{
    #region Interface: IAsyncRepository
    /// <summary>
    /// Provides an interface for asynchronous data operations on entities of type <typeparamref name="T"/>. 
    /// Inherits from <see cref="IQuery{T}"/> to support querying capabilities.
    /// </summary>
    /// <typeparam name="T">The type of the entity, which must inherit from <see cref="Entity"/>.</typeparam>
    public interface IAsyncRepository<T> : IQuery<T> where T : Entity
    {
        /// <summary>
        /// Retrieves a single entity based on a given predicate.
        /// </summary>
        /// <param name="predicate">An expression used to filter the entity to be retrieved.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the entity that matches the predicate.</returns>
        Task<T> GetAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Retrieves a paginated list of entities optionally filtered, ordered, and included with navigation properties.
        /// </summary>
        /// <param name="predicate">An optional expression for filtering the query.</param>
        /// <param name="orderBy">An optional function to order the query results.</param>
        /// <param name="include">An optional function to include related navigation properties.</param>
        /// <param name="index">The current page index for pagination.</param>
        /// <param name="size">The number of items per page.</param>
        /// <param name="enableTracking">Indicates whether change-tracking should be enabled.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result is a <see cref="IPaginate{T}"/> containing the paginated entities.</returns>
        Task<IPaginate<T>> GetListAsync(
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, 
            int index = 0, 
            int size = 10,
            bool enableTracking = true, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a paginated list of entities using dynamic filtering and optional navigation-property loading.
        /// </summary>
        /// <param name="dynamic">A <see cref="Dynamic"/> instance that defines filtering and sorting logic.</param>
        /// <param name="include">An optional function to include related navigation properties.</param>
        /// <param name="index">The current page index for pagination.</param>
        /// <param name="size">The number of items per page.</param>
        /// <param name="enableTracking">Indicates whether change-tracking should be enabled.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result is a <see cref="IPaginate{T}"/> containing the paginated entities.</returns>
        Task<IPaginate<T>> GetListByDynamicAsync(
            Dynamic dynamic,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, 
            int index = 0, 
            int size = 10,
            bool enableTracking = true, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Inserts a new entity into the data store.
        /// </summary>
        /// <param name="entity">The entity to be inserted.</param>
        /// <returns>A task that represents the asynchronous insert operation. 
        /// The task result contains the entity after being added.</returns>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Updates an existing entity in the data store.
        /// </summary>
        /// <param name="entity">The entity to be updated.</param>
        /// <returns>A task that represents the asynchronous update operation. 
        /// The task result contains the entity after being updated.</returns>
        Task<T> UpdateAsync(T entity);

        /// <summary>
        /// Removes an entity from the data store.
        /// </summary>
        /// <param name="entity">The entity to be removed.</param>
        /// <returns>A task that represents the asynchronous deletion operation. 
        /// The task result contains the entity after being removed.</returns>
        Task<T> DeleteAsync(T entity);
    }
    #endregion
}
#endregion
