#region Namespace

namespace ElasticSearch.Models.Concrete
{
    #region Class: ElasticSearchInsertUpdateModel
    /// <summary>
    /// Represents an insert or update request model for Elasticsearch. Inherits from <see cref="ElasticSearchModel"/>
    /// and includes the item data to be inserted or updated.
    /// </summary>
    public class ElasticSearchInsertUpdateModel : ElasticSearchModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the item data that will be inserted or updated in the Elasticsearch index.
        /// </summary>
        public object Item { get; set; }

        #endregion
    }
    #endregion
}
#endregion