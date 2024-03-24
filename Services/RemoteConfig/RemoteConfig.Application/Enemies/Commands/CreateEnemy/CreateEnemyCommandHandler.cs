using MediatR;
using RemoteConfig.Application.Interfaces;
using RemoteConfig.Core.Entities.Enemy;

namespace RemoteConfig.Application.Enemies.Commands.CreateEnemy;

public class CreateEnemyCommandHandler(IRemoteConfigContext dbContext) : IRequestHandler<CreateEnemyCommand, string>
{
    public async Task<string> Handle(CreateEnemyCommand request, CancellationToken cancellationToken)
    {
        var enemy = new Enemy()
        {
            Id = request.Id,
            
            Capacity = request.Capacity,
            Current = request.Current,
            Resistance = request.Resistance,
            Voltage = request.Voltage,
            
            EnemyType = request.EnemyType,
            AttackType = request.AttackType
        };

        await dbContext.Enemies.AddAsync(enemy, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return enemy.Id;
    }
}