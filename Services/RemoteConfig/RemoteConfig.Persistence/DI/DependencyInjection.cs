using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using RemoteConfig.Application.Interfaces;
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
            .AddDbContext<RemoteConfigContext>(options => 
                options.UseMongoDB(mongoClient, databaseName));
        services
            .AddScoped<IRemoteConfigContext>(provider =>
                provider.GetService<RemoteConfigContext>()
            );
        
        return services;
    }
}