using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using RemoteConfig.Application.Interfaces;

namespace RemoteConfig.Persistence.Caching;

public class CacheService(IMemoryCache memoryCache, IDistributedCache distributedCache) : ICacheService
{
    private const int MEMORY_CACHE_LIFETIME_SECONDS = 120;
    
    public async Task<T?> GetAsync<T>(string key, CancellationToken token = default) where T : class
    {
        memoryCache.TryGetValue<T>(key, out var inMemoryValue);
        if (inMemoryValue != null)
            return inMemoryValue;
        
        var inDisturbedValue = await distributedCache.GetStringAsync(key, token);
        
        if (inDisturbedValue != null)
            memoryCache.Set(
                key: key,
                value: JsonConvert.DeserializeObject<T>(inDisturbedValue),
                absoluteExpiration: DateTimeOffset.UtcNow.AddSeconds(MEMORY_CACHE_LIFETIME_SECONDS)
            );

        return inDisturbedValue == null
            ? null
            : JsonConvert.DeserializeObject<T>(inDisturbedValue);
    }

    public async Task<T> GetAsync<T>(string key, Func<Task<T>> factory, CancellationToken token = default) where T : class
    {
        var cached = await GetAsync<T>(key, token);
        
        if (cached != null) 
            return cached;
        
        cached ??= await factory();
        await SetAsync(key, cached, token);

        return cached;
    }

    public async Task SetAsync<T>(string key, T value, CancellationToken token = default) where T : class
    {
        var serialized = JsonConvert.SerializeObject(value);
        await distributedCache.SetStringAsync(key, serialized, token);
    }

    public async Task RemoveAsync(string key, CancellationToken token = default)
    {
        memoryCache.Remove(key);
        await distributedCache.RemoveAsync(key, token);
    }
}