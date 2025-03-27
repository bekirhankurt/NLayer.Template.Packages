#region Namespace

using ElasticSearch.Models.Abstract;
using ElasticSearch.Models.Concrete;
using Nest;

namespace ElasticSearch.Abstract
{
    #region Interface: IElasticSearch
    /// <summary>
    /// Defines operations for managing Elasticsearch indices and performing CRUD/search actions.
    /// </summary>
    public interface IElasticSearch
    {
        /// <summary>
        /// Creates a new index in Elasticsearch.
        /// </summary>
        /// <param name="indexModel">An <see cref="IndexModel"/> containing the name and settings of the index to be created.</param>
        /// <returns>An <see cref="IElasticSearchResult"/> indicating the result of the index creation operation.</returns>
        Task<IElasticSearchResult> CreateNewIndexAsync(IndexModel indexModel);

        /// <summary>
        /// Inserts a document into an Elasticsearch index.
        /// </summary>
        /// <param name="model">An <see cref="ElasticSearchInsertUpdateModel"/> containing the index name and the data to be inserted.</param>
        /// <returns>An <see cref="IElasticSearchResult"/> indicating the result of the insert operation.</returns>
        Task<IElasticSearchResult> InsertAsync(ElasticSearchInsertUpdateModel model);

        /// <summary>
        /// Inserts multiple documents into a specified Elasticsearch index.
        /// </summary>
        /// <param name="indexName">The name of the Elasticsearch index where the documents will be inserted.</param>
        /// <param name="items">An array of objects representing the documents to be inserted.</param>
        /// <returns>An <see cref="IElasticSearchResult"/> indicating the result of the bulk insert operation.</returns>
        Task<IElasticSearchResult> InsertManyAsync(string indexName, object[] items);

        /// <summary>
        /// Retrieves a dictionary of all Elasticsearch indices along with their corresponding states.
        /// </summary>
        /// <returns>A read-only dictionary mapping <see cref="IndexName"/> to <see cref="IndexState"/>.</returns>
        IReadOnlyDictionary<IndexName, IndexState> GetIndexList();

        /// <summary>
        /// Searches all documents in an Elasticsearch index based on the specified search parameters.
        /// </summary>
        /// <typeparam name="T">The type of documents to search.</typeparam>
        /// <param name="parameters">A <see cref="SearchParameters"/> object defining the search criteria.</param>
        /// <returns>A list of <see cref="ElasticSearchGetModel{T}"/> containing the matched documents.</returns>
        Task<List<ElasticSearchGetModel<T>>> GetAllSearchAsync<T>(SearchParameters parameters) where T : class;

        /// <summary>
        /// Performs a search on a specific field in an Elasticsearch index.
        /// </summary>
        /// <typeparam name="T">The type of documents to search.</typeparam>
        /// <param name="fieldParameters">A <see cref="SearchByFieldParameters"/> object defining the field and its search criteria.</param>
        /// <returns>A list of <see cref="ElasticSearchGetModel{T}"/> containing the matched documents.</returns>
        Task<List<ElasticSearchGetModel<T>>> GetSearchByField<T>(SearchByFieldParameters fieldParameters) where T : class;

        /// <summary>
        /// Searches documents using a simple query string in an Elasticsearch index.
        /// </summary>
        /// <typeparam name="T">The type of documents to search.</typeparam>
        /// <param name="queryParameters">A <see cref="SearchByQueryParameters"/> object defining the query string and its search criteria.</param>
        /// <returns>A list of <see cref="ElasticSearchGetModel{T}"/> containing the matched documents.</returns>
        Task<List<ElasticSearchGetModel<T>>> GetSearchBySimpleQueryString<T>(SearchByQueryParameters queryParameters) where T : class;

        /// <summary>
        /// Updates an existing Elasticsearch document by its Elasticsearch ID.
        /// </summary>
        /// <param name="model">An <see cref="ElasticSearchInsertUpdateModel"/> containing the index name and data to be updated.</param>
        /// <returns>An <see cref="IElasticSearchResult"/> indicating the result of the update operation.</returns>
        Task<IElasticSearchResult> UpdateByElasticIdAsync(ElasticSearchInsertUpdateModel model);

        /// <summary>
        /// Deletes an Elasticsearch document by its Elasticsearch ID.
        /// </summary>
        /// <param name="model">An <see cref="ElasticSearchModel"/> containing the index name and the ID of the document to delete.</param>
        /// <returns>An <see cref="IElasticSearchResult"/> indicating the result of the delete operation.</returns>
        Task<IElasticSearchResult> DeleteByElasticIdAsync(ElasticSearchModel model);
    }
    #endregion
}
#endregion
