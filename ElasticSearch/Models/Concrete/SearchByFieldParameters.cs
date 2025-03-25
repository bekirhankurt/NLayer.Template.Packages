#region Namespace

namespace ElasticSearch.Models.Concrete
{
    #region Class: SearchByFieldParameters
    /// <summary>
    /// Represents the parameters needed to execute a search on a specific field within an Elasticsearch index.
    /// Inherits from <see cref="SearchParameters"/>.
    /// </summary>
    public class SearchByFieldParameters : SearchParameters
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name of the field on which the search will be performed.
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Gets or sets the value to search for within the specified field.
        /// </summary>
        public string Value { get; set; }

        #endregion
    }
    #endregion
}
#endregion