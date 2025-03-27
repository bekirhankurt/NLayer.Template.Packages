#region Namespace
namespace Persistence.Paging.Abstract
{
    #region Interface: IPaginate
    /// <summary>
    /// Defines the contract for paginated results, encapsulating index information,
    /// page size, total counts, and navigation indicators.
    /// </summary>
    /// <typeparam name="T">The type of items held in the paginated collection.</typeparam>
    public interface IPaginate<T>
    {
        /// <summary>
        /// Gets the starting record index on the current page (usually zero-based).
        /// </summary>
        int From { get; }

        /// <summary>
        /// Gets the current page index (one-based or zero-based, depending on implementation).
        /// </summary>
        int Index { get; }

        /// <summary>
        /// Gets the number of items on each page.
        /// </summary>
        int Size { get; }

        /// <summary>
        /// Gets the total number of items across all pages.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets the total number of pages based on <see cref="Count"/> and <see cref="Size"/>.
        /// </summary>
        int Pages { get; }

        /// <summary>
        /// Gets the list of items on the current page.
        /// </summary>
        IList<T> Items { get; }

        /// <summary>
        /// Indicates whether there is a previous page.
        /// </summary>
        bool HasPrevious { get; }

        /// <summary>
        /// Indicates whether there is a next page.
        /// </summary>
        bool HasNext { get; }
    }
    #endregion
}
#endregion