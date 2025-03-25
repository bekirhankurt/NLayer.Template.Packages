#region Namespace

namespace ElasticSearch.Models.Concrete
{
    #region Class: IndexModel
    /// <summary>
    /// Represents the essential information required for creating or referencing
    /// an Elasticsearch index, including its name, alias, and shard/replica counts.
    /// </summary>
    public class IndexModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name of the Elasticsearch index.
        /// </summary>
        public string IndexName { get; set; }

        /// <summary>
        /// Gets or sets an alias to reference the index by a user-friendly name.
        /// </summary>
        public string AliasName { get; set; }

        /// <summary>
        /// Gets or sets the number of replicas for the index.
        /// Defaults to 3 if not explicitly assigned.
        /// </summary>
        public int NumberOfReplicas { get; set; } = 3;

        /// <summary>
        /// Gets or sets the number of primary shards for the index.
        /// Defaults to 3 if not explicitly assigned.
        /// </summary>
        public int NumberOfShards { get; set; } = 3;

        #endregion
    }
    #endregion
}
#endregion