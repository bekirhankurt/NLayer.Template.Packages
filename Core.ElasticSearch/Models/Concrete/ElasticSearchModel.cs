#region Usings
using Nest;
#endregion

#region Namespace

namespace ElasticSearch.Models.Concrete
{
    #region Class: ElasticSearchModel
    /// <summary>
    /// Represents a base model for Elasticsearch operations, including the identifier and index name.
    /// </summary>
    public class ElasticSearchModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Elasticsearch document identifier.
        /// </summary>
        public Id ElasticId { get; set; }

        /// <summary>
        /// Gets or sets the name of the Elasticsearch index in which the document resides.
        /// </summary>
        public string IndexName { get; set; }

        #endregion
    }
    #endregion
}
#endregion