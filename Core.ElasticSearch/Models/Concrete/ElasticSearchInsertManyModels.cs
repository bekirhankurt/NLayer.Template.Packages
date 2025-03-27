#region Namespace

namespace ElasticSearch.Models.Concrete
{
    #region Class: ElasticSearchInsertManyModels
    /// <summary>
    /// Represents a bulk insert request model for Elasticsearch. Inherits from <see cref="ElasticSearchModel"/>
    /// and adds an array of items to be inserted.
    /// </summary>
    public class ElasticSearchInsertManyModels : ElasticSearchModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the array of objects that will be inserted into the Elasticsearch index.
        /// </summary>
        public object[] Items { get; set; }

        #endregion
    }
    #endregion
}
#endregion