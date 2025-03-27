#region Namespace

namespace ElasticSearch.Models.Concrete
{
    #region Class: SearchParameters
    /// <summary>
    /// Represents basic search parameters used for Elasticsearch queries,
    /// including the target index and pagination options.
    /// </summary>
    public class SearchParameters
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name of the Elasticsearch index to search against.
        /// </summary>
        public string IndexName { get; set; }

        /// <summary>
        /// Gets or sets the starting offset for paginated results. Defaults to 0.
        /// </summary>
        public int From { get; set; } = 0;

        /// <summary>
        /// Gets or sets the number of results to retrieve per page. Defaults to 10.
        /// </summary>
        public int Size { get; set; } = 10;

        #endregion
    }
    #endregion
}
#endregion