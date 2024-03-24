using MediatR;
using Microsoft.EntityFrameworkCore;
using RemoteConfig.Application.Common.Exceptions;
using RemoteConfig.Application.Interfaces;
using RemoteConfig.Core.Entities.Enemy;

namespace RemoteConfig.Application.Enemies.Commands.UpdateEnemy;

public class UpdateEnemyCommandHandler(IRemoteConfigContext dbContext) : IRequestHandler<UpdateEnemyCommand>
{
    public async Task Handle(UpdateEnemyCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Enemies.FirstOrDefaultAsync(
            h => h.Id == request.Id, cancellationToken);
        
        if (entity == null)
            throw new NotFoundException(nameof(Enemy), request.Id);

        entity.Capacity   = request.Capacity;
        entity.Current    = request.Current;
        entity.Resistance = request.Resistance;
        entity.Voltage    = request.Voltage;

        entity.EnemyType  = request.EnemyType;
        entity.AttackType = request.AttackType;
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}