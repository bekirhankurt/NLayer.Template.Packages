#region Namespace

namespace ElasticSearch.Models.Concrete
{
    #region Class: ElasticSearchGetModel
    /// <summary>
    /// Represents a generic result item retrieved from an Elasticsearch index,
    /// including its document identifier and the item itself.
    /// </summary>
    /// <typeparam name="T">The type of the retrieved document.</typeparam>
    public class ElasticSearchGetModel<T>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Elasticsearch document identifier.
        /// </summary>
        public string ElasticId { get; set; }

        /// <summary>
        /// Gets or sets the deserialized document from Elasticsearch.
        /// </summary>
        public T Item { get; set; }

        #endregion
    }
    #endregion
}
#endregion