using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MongoDB.EntityFrameworkCore.Extensions;
using RemoteConfig.Application.Interfaces;
using RemoteConfig.Core.Entities.Enemy;
using RemoteConfig.Core.Entities.Hero;
using RemoteConfig.Core.Entities.Inventory;
using RemoteConfig.Core.Entities.Stage;

namespace RemoteConfig.Persistence.Database;

public class RemoteConfigContext(DbContextOptions<RemoteConfigContext> options, IConfiguration configuration)
    : DbContext(options), IRemoteConfigContext
{
    private const string STAGES_COLLECTION  = "DatabaseSettings:StagesCollection";
    private const string HEROES_COLLECTIONS = "DatabaseSettings:HeroesCollections";
    private const string ENEMIES_COLLECTION = "DatabaseSettings:EnemiesCollection";
    private const string ITEMS_COLLECTION   = "DatabaseSettings:InventoryItemsCollection";

    public DbSet<Stage> Stages { get; set; }
    public DbSet<Hero> Heroes { get; set; }
    public DbSet<Enemy> Enemies { get; set; }
    public DbSet<InventoryItem> InventoryItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .Entity<Stage>()
            .ToCollection(configuration.GetSection(STAGES_COLLECTION).Value);
        modelBuilder
            .Entity<Hero>()
            .ToCollection(configuration.GetSection(HEROES_COLLECTIONS).Value);
        modelBuilder
            .Entity<Enemy>()
            .ToCollection(configuration.GetSection(ENEMIES_COLLECTION).Value);
        modelBuilder
            .Entity<InventoryItem>()
            .ToCollection(configuration.GetSection(ITEMS_COLLECTION).Value);
    }
}