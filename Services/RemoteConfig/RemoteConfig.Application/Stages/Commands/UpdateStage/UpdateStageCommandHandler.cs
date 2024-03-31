using MediatR;
using Microsoft.EntityFrameworkCore;
using RemoteConfig.Application.Common.Exceptions;
using RemoteConfig.Application.Interfaces;
using RemoteConfig.Core.Entities.Stage;

namespace RemoteConfig.Application.Stages.Commands.UpdateStage;

public class UpdateStageCommandHandler(IRemoteConfigContext dbContext) : IRequestHandler<UpdateStageCommand>
{
    public async Task Handle(UpdateStageCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Stages.FirstOrDefaultAsync(
            h => h.Id == request.Id, cancellationToken);
        
        if (entity == null)
            throw new NotFoundException(nameof(Stage), request.Id);

        entity.StageTitle       = request.StageTitle;
        entity.StageDescription = request.StageDescription;
        entity.BoardTiles       = request.BoardTiles;
        entity.PlayerSpawnPoint = request.PlayerSpawnPoint;
        entity.EnemySpawners    = request.EnemySpawners;
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}