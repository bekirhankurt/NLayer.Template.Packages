namespace Application.Pipelines.Caching;

public interface ICacheRemoveRequest
{
    bool BypassCache { get; }
    string CacheKey { get; }
}