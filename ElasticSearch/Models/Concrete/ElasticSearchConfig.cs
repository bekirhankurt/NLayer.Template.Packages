#region Namespace

namespace ElasticSearch.Models.Concrete
{
    #region Class: ElasticSearchConfig
    /// <summary>
    /// Represents configuration data for connecting to an Elasticsearch instance,
    /// including the connection string and optional authentication credentials.
    /// </summary>
    public class ElasticSearchConfig
    {
        #region Properties

        /// <summary>
        /// Gets or sets the connection string to the Elasticsearch instance.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the username credential for Elasticsearch authentication.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password credential for Elasticsearch authentication.
        /// </summary>
        public string Password { get; set; }

        #endregion
    }
    #endregion
}
#endregion