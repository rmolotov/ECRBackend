using MediatR;
using RemoteConfig.Application.Common.Exceptions;
using RemoteConfig.Application.Interfaces;
using RemoteConfig.Core.Entities.Enemy;

namespace RemoteConfig.Application.Enemies.Commands.DeleteEnemy;

public class DeleteEnemyCommandHandler(IRemoteConfigContext dbContext) : IRequestHandler<DeleteEnemyCommand>
{
    public async Task Handle(DeleteEnemyCommand request, CancellationToken cancellationToken)
    {
        var enemy = await dbContext.Enemies.FindAsync(
            [request.Id],
            cancellationToken);

        if (enemy == null)
            throw new NotFoundException(nameof(Enemy), request.Id);

        dbContext.Enemies.Remove(enemy);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}