#region Namespace

namespace ElasticSearch.Models.Abstract
{
    #region Interface: IElasticSearchResult
    /// <summary>
    /// Defines the structure of a result returned from Elasticsearch operations,
    /// indicating success and providing a message for further detail.
    /// </summary>
    public interface IElasticSearchResult
    {
        /// <summary>
        /// Gets a value indicating whether the operation was successful.
        /// </summary>
        bool Success { get; }

        /// <summary>
        /// Gets a message describing the outcome of the operation.
        /// </summary>
        string Message { get; }
    }
    #endregion
}
#endregion