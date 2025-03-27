#region Usings
using ElasticSearch.Abstract;
using ElasticSearch.Models.Abstract;
using ElasticSearch.Models.Concrete;
using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Nest;
using Nest.JsonNetSerializer;
using Newtonsoft.Json;
#endregion

#region Namespace

namespace ElasticSearch.Concrete
{
    
    //TODO: Refactor Success Messages.
    
    #region Class: ElasticSearchService
    /// <summary>
    /// Implements <see cref="IElasticSearch"/> to manage and query Elasticsearch indices,
    /// including creation, bulk insert, searching, updating, and deletion of documents.
    /// </summary>
    public class ElasticSearchService : IElasticSearch
    {
        #region Fields

        /// <summary>
        /// Holds the <see cref="ConnectionSettings"/> to connect to Elasticsearch.
        /// </summary>
        private readonly ConnectionSettings _connectionSettings;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of <see cref="ElasticSearchService"/> using the provided configuration.
        /// Retrieves Elasticsearch connection details from the "ElasticSearchConfig" section in <paramref name="configuration"/>.
        /// </summary>
        /// <param name="configuration">The application configuration object that contains "ElasticSearchConfig" settings.</param>
        public ElasticSearchService(IConfiguration configuration)
        {
            var settings = configuration.GetSection("ElasticSearchConfig").Get<ElasticSearchConfig>();

            var pool = new SingleNodeConnectionPool(new Uri(settings.ConnectionString));
            _connectionSettings = new ConnectionSettings(pool, (builtInSerializer, connectionSettings) =>
                new JsonNetSerializer(
                    builtInSerializer, connectionSettings, () => new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }));
        }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public async Task<IElasticSearchResult> CreateNewIndexAsync(IndexModel indexModel)
        {
            var elasticClient = GetElasticClient(indexModel.IndexName);

            if (await elasticClient.Indices.ExistsAsync(indexModel.IndexName) != null)
                return new ElasticSearchResult(false, "Index Already Exists!");

            var response = await elasticClient.Indices.CreateAsync(
                indexModel.IndexName,
                se => se.Settings(s =>
                        s.NumberOfReplicas(indexModel.NumberOfReplicas)
                         .NumberOfShards(indexModel.NumberOfShards))
                      .Aliases(x => x.Alias(indexModel.AliasName)));

            return new ElasticSearchResult(
                response.IsValid,
                response.IsValid ? "Success" : response.ServerError?.Error?.Reason);
        }

        /// <inheritdoc />
        public async Task<IElasticSearchResult> InsertAsync(ElasticSearchInsertUpdateModel model)
        {
            var elasticClient = GetElasticClient(model.IndexName);
            var response = await elasticClient.IndexAsync(model.Item, i => i.Index(model.IndexName));

            return new ElasticSearchResult(
                response.IsValid,
                response.IsValid ? "Success" : response.ServerError?.Error?.Reason);
        }

        /// <inheritdoc />
        public async Task<IElasticSearchResult> InsertManyAsync(string indexName, object[] items)
        {
            var elasticClient = GetElasticClient(indexName);
            var response = await elasticClient.BulkAsync(r => r.Index(indexName).IndexMany(items));

            return new ElasticSearchResult(
                response.IsValid,
                response.IsValid ? "Success" : response.ServerError?.Error?.Reason);
        }

        /// <inheritdoc />
        public IReadOnlyDictionary<IndexName, IndexState> GetIndexList()
        {
            var elasticClient = new ElasticClient(_connectionSettings);
            return elasticClient.Indices.Get(new GetIndexRequest(Indices.All)).Indices;
        }

        /// <inheritdoc />
        public async Task<List<ElasticSearchGetModel<T>>> GetAllSearchAsync<T>(SearchParameters parameters) where T : class
        {
            var elasticClient = GetElasticClient(parameters.IndexName);
            var searchResponse = await elasticClient.SearchAsync<T>(s =>
                s.Index(Indices.Index(parameters.IndexName))
                 .From(parameters.From)
                 .Size(parameters.Size));

            return searchResponse.Hits
                .Select(h => new ElasticSearchGetModel<T>
                {
                    ElasticId = h.Id,
                    Item = h.Source
                })
                .ToList();
        }

        /// <inheritdoc />
        public async Task<List<ElasticSearchGetModel<T>>> GetSearchByField<T>(SearchByFieldParameters fieldParameters)
            where T : class
        {
            var elasticClient = GetElasticClient(fieldParameters.IndexName);
            var response = await elasticClient.SearchAsync<T>(s => s
                .Index(fieldParameters.IndexName)
                .From(fieldParameters.From)
                .Size(fieldParameters.Size));

            return response.Hits
                .Select(h => new ElasticSearchGetModel<T>
                {
                    ElasticId = h.Id,
                    Item = h.Source
                })
                .ToList();
        }

        /// <inheritdoc />
        public async Task<List<ElasticSearchGetModel<T>>> GetSearchBySimpleQueryString<T>(SearchByQueryParameters queryParameters) 
            where T : class
        {
            var elasticClient = GetElasticClient(queryParameters.IndexName);
            var response = await elasticClient.SearchAsync<T>(s => s
                .Index(queryParameters.IndexName)
                .From(queryParameters.From)
                .Size(queryParameters.Size)
                .MatchAll()
                .Query(q => q.SimpleQueryString(sq => sq
                    .Name(queryParameters.QueryName)
                    .Boost(1.1)
                    .Fields(queryParameters.Fields)
                    .Query(queryParameters.Query)
                    .Analyzer("standard")
                    .DefaultOperator(Operator.Or)
                    .Flags(SimpleQueryStringFlags.And | SimpleQueryStringFlags.Near)
                    .Lenient()
                    .AnalyzeWildcard(false)
                    .MinimumShouldMatch("30%")
                    .FuzzyPrefixLength(0)
                    .FuzzyMaxExpansions(50)
                    .FuzzyTranspositions()
                    .AutoGenerateSynonymsPhraseQuery(false))));

            return response.Hits
                .Select(h => new ElasticSearchGetModel<T>
                {
                    ElasticId = h.Id,
                    Item = h.Source
                })
                .ToList();
        }

        /// <inheritdoc />
        public async Task<IElasticSearchResult> UpdateByElasticIdAsync(ElasticSearchInsertUpdateModel model)
        {
            var elasticClient = GetElasticClient(model.IndexName);
            var response = await elasticClient.UpdateAsync<object>(
                model.ElasticId,
                u => u.Index(model.IndexName).Doc(model.Item));

            return new ElasticSearchResult(
                response.IsValid,
                response.IsValid ? "Success" : response.ServerError?.Error?.Reason);
        }

        /// <inheritdoc />
        public async Task<IElasticSearchResult> DeleteByElasticIdAsync(ElasticSearchModel model)
        {
            var elasticClient = GetElasticClient(model.IndexName);
            var response = await elasticClient.DeleteAsync<object>(model.ElasticId, x => x.Index(model.IndexName));

            return new ElasticSearchResult(
                response.IsValid,
                response.IsValid ? "Success" : response.ServerError?.Error?.Reason);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Retrieves an instance of <see cref="ElasticClient"/> based on the provided index name.
        /// </summary>
        /// <param name="indexName">The name of the Elasticsearch index to be used by the client.</param>
        /// <returns>An <see cref="ElasticClient"/> configured with <see cref="_connectionSettings"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="indexName"/> is null or empty.</exception>
        private ElasticClient GetElasticClient(string indexName)
        {
            if (string.IsNullOrEmpty(indexName))
                throw new ArgumentNullException(indexName, "Index name cannot be null or empty.");

            return new ElasticClient(_connectionSettings);
        }

        #endregion
    }
    #endregion
}
#endregion
