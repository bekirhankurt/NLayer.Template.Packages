using System.Text;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Application.Pipelines.Caching;

public class CacheRemovingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
where TRequest : IRequest<TResponse>, ICacheRemoveRequest
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<CacheRemovingBehavior<TRequest, TResponse>> _logger; 
    public CacheRemovingBehavior(ILogger<CacheRemovingBehavior<TRequest, TResponse>> logger, IDistributedCache cache)
    {
        _logger = logger;
        _cache = cache;
    }
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request.BypassCache) return await next();

        var response = await GetResponseAndRemoveCache();
        _logger.LogInformation(new StringBuilder().Append("Removed cache --> ").Append(request.CacheKey).ToString());
        return response;

        async Task<TResponse> GetResponseAndRemoveCache()
        {
            response = await next();
            await _cache.RemoveAsync(request.CacheKey, cancellationToken);
            return response;
        }
    }
}