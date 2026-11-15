using Microsoft.Extensions.DependencyInjection;
using RemoteConfig.Persistence.Caching.Providers;

namespace RemoteConfig.Persistence.Caching.Extensions;

public class CacheConfigurationExpression 
{
    private readonly List<ICacheProvider> _providers = [];
    private IServiceProvider _serviceProvider;

    public IReadOnlyCollection<ICacheProvider> Providers => _providers;

    public void ConfigServiceProvider(IServiceProvider provider)
    {
        _serviceProvider = provider;
    }

    public void AddProvider<TProvider>() where TProvider : ICacheProvider
    {
        var provider = ActivatorUtilities.CreateInstance<TProvider>(_serviceProvider);
        _providers.Add(provider);
    }
}