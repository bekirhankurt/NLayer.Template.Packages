#region Namespace

namespace ElasticSearch.Models.Concrete
{
    #region Class: SearchByQueryParameters
    /// <summary>
    /// Defines the parameters needed to perform a search using a simple query string in an Elasticsearch index.
    /// Inherits from <see cref="SearchParameters"/>.
    /// </summary>
    public class SearchByQueryParameters : SearchParameters
    {
        #region Properties
        
        /// <summary>
        /// Gets or sets a name to identify the query (useful in logging or tracing).
        /// </summary>
        public string QueryName { get; set; }

        /// <summary>
        /// Gets or sets the query string to be used in the simple query string search.
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// Gets or sets an array of field names to which this query applies.
        /// </summary>
        public string[] Fields { get; set; }

        #endregion
    }
    #endregion
}
#endregion