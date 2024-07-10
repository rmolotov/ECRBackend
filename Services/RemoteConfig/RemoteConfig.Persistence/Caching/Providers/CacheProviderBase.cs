namespace RemoteConfig.Persistence.Caching.Providers;

public abstract class CacheProviderBase : ICacheProvider
{
    public ICacheProvider? Successor { get; set; }
    
    public virtual async Task<T?> GetAsync<T>(string key, CancellationToken token = default) where T : class
    {
        if (Successor == null) 
            return null;
        
        var next = await Successor.GetAsync<T>(key, token);
        
        if (next != null)
            await SetAsync(key, next, token);
        
        return next;
    }

    public abstract Task SetAsync<T>(string key, T value, CancellationToken token = default) where T : class;
    
    public virtual async Task RemoveAsync(string key, CancellationToken token = default)
    {
        if (Successor != null)
            await Successor.RemoveAsync(key, token);
    }
}