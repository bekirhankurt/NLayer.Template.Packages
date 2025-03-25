#region Usings
using System.Diagnostics.CodeAnalysis;
using ElasticSearch.Models.Abstract;
#endregion

#region Namespace

namespace ElasticSearch.Models.Concrete
{
    #region Class: ElasticSearchResult
    /// <summary>
    /// Represents the outcome of an Elasticsearch operation, including success status and a descriptive message.
    /// Implements <see cref="IElasticSearchResult"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ElasticSearchResult : IElasticSearchResult
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ElasticSearchResult"/> class 
        /// with a specified success value and message.
        /// </summary>
        /// <param name="success">Indicates whether the operation was successful.</param>
        /// <param name="message">A message describing the result of the operation.</param>
        public ElasticSearchResult(bool success, string message)
            : this(success)
        {
            Message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ElasticSearchResult"/> class 
        /// with a specified success value.
        /// </summary>
        /// <param name="success">Indicates whether the operation was successful.</param>
        public ElasticSearchResult(bool success)
        {
            Success = success;
        }

        #endregion

        #region Properties

        /// <inheritdoc/>
        public bool Success { get; }

        /// <inheritdoc/>
        public string Message { get; }

        #endregion
    }
    #endregion
}
#endregion