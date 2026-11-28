using RemoteConfig.Application.Interfaces;
using RemoteConfig.Persistence.Caching.Extensions;
using RemoteConfig.Persistence.Caching.Providers;

namespace RemoteConfig.Persistence.Caching;

public class CacheService : ICacheService
{
    private readonly ICacheProvider _head;
    private readonly ICacheProvider _tail;

    public CacheService(CacheConfigurationExpression configurationExpression)
    {
        var providers = configurationExpression.Providers.ToArray();
        
        _head = providers[0];
        _tail = providers[^1];
        _head.Successor = _tail;
        
        if (providers.Length < 2)
            return;

        for (var i = 0; i < providers.Length - 1; i++) 
            providers[i].Successor = providers[i + 1];
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken token = default) where T : class => 
        await _head.GetAsync<T>(key, token);

    public async Task SetAsync<T>(string key, T value, CancellationToken token = default) where T : class => 
        await _tail.SetAsync(key, value, token);

    public async Task RemoveAsync(string key, CancellationToken token = default) => 
        await _head.RemoveAsync(key, token);
}