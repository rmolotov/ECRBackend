namespace RemoteConfig.Persistence.Caching.Providers;

public interface ICacheProvider
{
    ICacheProvider? Successor { get; set; }

    Task<T?> GetAsync<T>(string key, CancellationToken token = default) where T : class;
    Task SetAsync<T>(string key, T value, CancellationToken token = default) where T : class;
    Task RemoveAsync(string key, CancellationToken token = default);
}