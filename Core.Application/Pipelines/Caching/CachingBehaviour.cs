using System.Text;
using System.Xml;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Application.Pipelines.Caching;

public class CachingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
where TRequest : IRequest<TResponse>, ICacheableRequest
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<CachingBehaviour<TRequest, TResponse>> _logger;

    private readonly CacheSettings _cacheSettings;


    public CachingBehaviour(IDistributedCache cache, ILogger<CachingBehaviour<TRequest, TResponse>> logger, CacheSettings cacheSettings, IConfiguration configuration)
    {
        _cache = cache;
        _logger = logger;
        _cacheSettings = configuration.GetSection("CacheSettings").Get<CacheSettings>();
    }


    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        TResponse response = default;

        if (request.BypassCache) return await next();

        var cachedResponse = await _cache.GetAsync(request.CacheKey, cancellationToken);

        if (cachedResponse is not null)
        {
            response = JsonConvert.DeserializeObject<TResponse>(Encoding.Default.GetString(cachedResponse));
            _logger.LogInformation(new StringBuilder().Append("Fetched from cache --> ")
                .Append(request.CacheKey)
                .ToString());
        }
        else
        {
            response = await GetResponseAndAddItToTheCache();
            _logger.LogInformation(
                new StringBuilder().Append("Added to cache --> ").Append(request.CacheKey).ToString());
        }

        return response;

        async Task<TResponse> GetResponseAndAddItToTheCache()
        {
            TimeSpan? slidingExpiration = request.SlidingExpiration ?? TimeSpan.FromDays(_cacheSettings.SlidingExpiration);
            var cacheOptions = new DistributedCacheEntryOptions
            {
                SlidingExpiration = slidingExpiration
            };

            var serializeData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));
            await _cache.SetAsync(request.CacheKey, serializeData, cacheOptions, cancellationToken);
            return response;
        }
    }
}