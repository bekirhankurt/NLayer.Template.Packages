#region Namespace

namespace Persistence.Repositories.Abstract
{
    #region Interface: IQuery
    /// <summary>
    /// Defines a contract for obtaining an <see cref="IQueryable{T}"/> sequence of entities.
    /// </summary>
    /// <typeparam name="T">The type of entity to query.</typeparam>
    public interface IQuery<out T>
    {
        /// <summary>
        /// Returns an <see cref="IQueryable{T}"/> enabling further composition of queries.
        /// </summary>
        /// <returns>An <see cref="IQueryable{T}"/> for the current entity set.</returns>
        IQueryable<T> Query();
    }
    #endregion
}
#endregion