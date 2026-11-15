using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace RemoteConfig.Persistence.Caching.Providers;

public class RedisCacheProvider(IDistributedCache distributedCache) : CacheProviderBase
{
    public override async Task<T?> GetAsync<T>(string key, CancellationToken token = default) where T : class
    {
        var inDisturbedValue = await distributedCache.GetStringAsync(key, token);

        if (inDisturbedValue != null)
            return JsonConvert.DeserializeObject<T>(inDisturbedValue);

        return await base.GetAsync<T>(key, token);
    }

    public override async Task SetAsync<T>(string key, T value, CancellationToken token = default) where T : class
    {
        var serialized = JsonConvert.SerializeObject(value);
        
        await distributedCache.SetStringAsync(
            key: key,
            value: serialized,
            token: token);
    }

    public override async Task RemoveAsync(string key, CancellationToken token = default)
    {
        await distributedCache.RemoveAsync(key, token);
        await base.RemoveAsync(key, token);
    }
}