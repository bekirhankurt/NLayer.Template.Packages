#region Namespace

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Persistence.Dynamics.Concrete;
using Persistence.Paging.Abstract;
using Persistence.Repositories.Concrete;

namespace Persistence.Repositories.Abstract
{
    #region Interface: IRepository
    /// <summary>
    /// Defines a contract for synchronous data repository operations on entities of type <typeparamref name="T"/>. 
    /// Implements <see cref="IQuery{T}"/> to support querying capabilities.
    /// </summary>
    /// <typeparam name="T">The entity type, which must inherit from <see cref="Entity"/>.</typeparam>
    public interface IRepository<T> : IQuery<T> where T : Entity
    {
        /// <summary>
        /// Retrieves a single entity based on the specified predicate.
        /// </summary>
        /// <param name="predicate">An expression to filter the entity to be retrieved.</param>
        /// <returns>The entity that matches the predicate, or null if no entity is found.</returns>
        T? Get(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Retrieves a paginated list of entities, optionally filtered, ordered, and including related objects.
        /// </summary>
        /// <param name="predicate">An optional expression to filter the entities.</param>
        /// <param name="orderBy">An optional function to order the query results.</param>
        /// <param name="include">An optional function to include related navigation properties in the query.</param>
        /// <param name="index">The current page index for pagination.</param>
        /// <param name="size">The number of items per page.</param>
        /// <param name="enableTracking">Indicates whether change-tracking should be enabled.</param>
        /// <returns>An <see cref="IPaginate{T}"/> containing the paginated entities.</returns>
        IPaginate<T> GetList(
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            int index = 0, 
            int size = 10,
            bool enableTracking = true);

        /// <summary>
        /// Retrieves a paginated list of entities using a <see cref="Dynamic"/> object 
        /// to define filtering and sorting, optionally including navigation properties.
        /// </summary>
        /// <param name="dynamic">A <see cref="Dynamic"/> instance defining filter and sort criteria.</param>
        /// <param name="include">An optional function to include related navigation properties in the query.</param>
        /// <param name="index">The current page index for pagination.</param>
        /// <param name="size">The number of items per page.</param>
        /// <param name="enableTracking">Indicates whether change-tracking should be enabled.</param>
        /// <returns>An <see cref="IPaginate{T}"/> containing the paginated entities.</returns>
        IPaginate<T> GetListByDynamic(
            Dynamic dynamic,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            int index = 0, 
            int size = 10, 
            bool enableTracking = true);

        /// <summary>
        /// Inserts a new entity into the data store.
        /// </summary>
        /// <param name="entity">The entity to be inserted.</param>
        /// <returns>The inserted entity.</returns>
        T Add(T entity);

        /// <summary>
        /// Updates an existing entity in the data store.
        /// </summary>
        /// <param name="entity">The entity to be updated.</param>
        /// <returns>The updated entity.</returns>
        T Update(T entity);

        /// <summary>
        /// Removes an entity from the data store.
        /// </summary>
        /// <param name="entity">The entity to be deleted.</param>
        /// <returns>The deleted entity.</returns>
        T Delete(T entity);
    }
    #endregion
}
#endregion
