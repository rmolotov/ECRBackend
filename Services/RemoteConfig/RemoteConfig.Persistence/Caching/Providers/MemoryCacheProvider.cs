using Microsoft.Extensions.Caching.Memory;

namespace RemoteConfig.Persistence.Caching.Providers;

public class MemoryCacheProvider(IMemoryCache memoryCache) : CacheProviderBase
{
    private const int MEMORY_CACHE_LIFETIME_SECONDS = 10;

    public override async Task<T?> GetAsync<T>(string key, CancellationToken token = default) where T : class
    {
        return memoryCache.TryGetValue<T>(key, out var inMemoryValue)
            ? inMemoryValue
            : await base.GetAsync<T>(key, token);
    }

    public override async Task SetAsync<T>(string key, T value, CancellationToken token = default) where T : class
    {
        memoryCache.Set(
            key: key,
            value: value,
            absoluteExpiration: DateTimeOffset.UtcNow.AddSeconds(MEMORY_CACHE_LIFETIME_SECONDS)
        );
    }

    public override async Task RemoveAsync(string key, CancellationToken token = default)
    {
        memoryCache.Remove(key);
        await base.RemoveAsync(key, token);
    }
}