using Microsoft.EntityFrameworkCore;
using RemoteConfig.Core.Entities.Enemy;
using RemoteConfig.Core.Entities.Hero;
using RemoteConfig.Core.Entities.Inventory;
using RemoteConfig.Core.Entities.Stage;

namespace RemoteConfig.Application.Interfaces;

public interface IRemoteConfigContext
{
    DbSet<Stage> Stages { get; set; }
    DbSet<Hero> Heroes { get; set; }
    DbSet<Enemy> Enemies { get; set; }
    DbSet<InventoryItem> InventoryItems { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}