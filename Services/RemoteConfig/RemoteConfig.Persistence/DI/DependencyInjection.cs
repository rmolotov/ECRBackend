using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RemoteConfig.Application.Interfaces;
using RemoteConfig.Persistence.Caching;
using RemoteConfig.Persistence.Caching.Extensions;
using RemoteConfig.Persistence.Database;

namespace RemoteConfig.Persistence.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetSection("DatabaseSettings:ConnectionString").Value;
        var databaseName = configuration.GetSection("DatabaseSettings:DatabaseName").Value;
        
        var mongoClient = new MongoClient(connectionString);

        services
            .AddDbContext<IRemoteConfigContext, RemoteConfigContext>(options => options
                .UseMongoDB(mongoClient, databaseName));

        return services;
    }

    public static IServiceCollection AddCaching(this IServiceCollection services, Action<CacheConfigurationExpression> setupAction)
    {
        services
            .AddMemoryCache()
            .AddDistributedMemoryCache()
            .AddCacheProviders(
                (sp, cfg) =>
                {
                    cfg.ConfigServiceProvider(sp);
                    setupAction.Invoke(cfg);
                })
            .AddSingleton<ICacheService>(provider =>
            {
                var options = provider.GetRequiredService<IOptions<CacheConfigurationExpression>>();
                return new CacheService(options.Value);
            });

        return services;
    }

    private static IServiceCollection AddCacheProviders(this IServiceCollection services,
        Action<IServiceProvider, CacheConfigurationExpression> configAction)
    {
        services
            .AddOptions<CacheConfigurationExpression>()
            .Configure<IServiceProvider>((options, sp) => configAction(sp, options));

        return services;
    }
}