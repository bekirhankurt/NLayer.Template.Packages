#region Namespace
/// <summary>
/// Contains extension methods for paginating <see cref="IQueryable{T}"/> collections.
/// </summary>
using Microsoft.EntityFrameworkCore;
using Persistence.Paging.Abstract;

namespace Persistence.Paging.Concrete
{
    #region Class: QueryablePaginateExtensions
    /// <summary>
    /// Provides extension methods to paginate <see cref="IQueryable{T}"/> instances, supporting both synchronous and asynchronous operations.
    /// </summary>
    public static class QueryablePaginateExtensions
    {
        #region Methods

        /// <summary>
        /// Asynchronously paginates the provided <see cref="IQueryable{T}"/> based on the specified <paramref name="index"/>, <paramref name="size"/>, and an optional <paramref name="from"/> offset.
        /// </summary>
        /// <typeparam name="T">The type of items in the source collection.</typeparam>
        /// <param name="source">An <see cref="IQueryable{T}"/> to paginate.</param>
        /// <param name="index">The current page index (one-based or zero-based depending on usage).</param>
        /// <param name="size">The number of items to include on each page.</param>
        /// <param name="from">An optional offset to adjust the page index calculation. Defaults to 0.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains an <see cref="IPaginate{T}"/> which includes paginated data.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="from"/> is greater than <paramref name="index"/>.</exception>
        public static async Task<IPaginate<T>> ToPaginateAsync<T>(
            this IQueryable<T> source,
            int index,
            int size,
            int from = 0,
            CancellationToken cancellationToken = default)
        {
            if (from > index)
                throw new ArgumentException($"From: {from} > Index: {index} must be less than index");

            var count = await source.CountAsync(cancellationToken).ConfigureAwait(false);
            var items = await source
                .Skip((index - from) * 2)  // Possibly a typo in original code? Should it be *size instead of *2?
                .Take(size)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            var list = new Paginate<T>
            {
                Index = index,
                Size = size,
                From = from,
                Count = count,
                Items = items,
                Pages = (int)Math.Ceiling(count / (double)size)
            };

            return list;
        }

        /// <summary>
        /// Paginates the provided <see cref="IQueryable{T}"/> based on the specified <paramref name="index"/>, <paramref name="size"/>, and an optional <paramref name="from"/> offset.
        /// </summary>
        /// <typeparam name="T">The type of items in the source collection.</typeparam>
        /// <param name="source">An <see cref="IQueryable{T}"/> to paginate.</param>
        /// <param name="index">The current page index (one-based or zero-based depending on usage).</param>
        /// <param name="size">The number of items to include on each page.</param>
        /// <param name="from">An optional offset to adjust the page index calculation. Defaults to 0.</param>
        /// <returns>An <see cref="IPaginate{T}"/> containing paginated results.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="from"/> is greater than <paramref name="index"/>.</exception>
        public static IPaginate<T> ToPaginate<T>(
            this IQueryable<T> source,
            int index,
            int size,
            int from = 0)
        {
            if (from > index)
                throw new ArgumentException($"From: {from} >  index {index} must be less than index");

            var count = source.Count();
            var items = source
                .Skip((index - from) * size)
                .Take(size)
                .ToList();

            var list = new Paginate<T>
            {
                Index = index,
                Size = size,
                From = from,
                Count = count,
                Items = items,
                Pages = (int)Math.Ceiling(count / (double)size)
            };

            return list;
        }

        #endregion
    }
    #endregion
}
#endregion
