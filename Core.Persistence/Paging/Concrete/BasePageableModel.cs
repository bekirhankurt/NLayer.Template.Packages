#region Namespace

namespace Persistence.Paging.Concrete
{
    #region Class: BasePageableModel
    /// <summary>
    /// Represents a base paginated model, including properties for indexing and navigation.
    /// </summary>
    public abstract class BasePageableModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the current page index.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets the number of items per page.
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Gets or sets the total number of items across all pages.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets the total number of pages calculated from <see cref="Count"/> and <see cref="Size"/>.
        /// </summary>
        public int Pages { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there is a previous page.
        /// </summary>
        public bool HasPrevious { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there is a next page.
        /// </summary>
        public bool HasNext { get; set; }

        #endregion
    }
    #endregion
}
#endregion