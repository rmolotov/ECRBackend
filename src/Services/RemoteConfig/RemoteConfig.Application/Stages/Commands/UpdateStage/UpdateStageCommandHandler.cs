using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RemoteConfig.Application.Common.Exceptions;
using RemoteConfig.Application.Interfaces;
using RemoteConfig.Core.Entities.Stage;

namespace RemoteConfig.Application.Stages.Commands.UpdateStage;

public class UpdateStageCommandHandler(IRemoteConfigContext dbContext, IMapper mapper) : IRequestHandler<UpdateStageCommand>
{
    public async Task Handle(UpdateStageCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Stages.FirstOrDefaultAsync(
            h => h.Id == request.Id, cancellationToken);
        
        if (entity == null)
            throw new NotFoundException(nameof(Stage), request.Id);

        var edited = mapper.Map<Stage>(request);
        
        entity.StageTitle       = edited.StageTitle;
        entity.StageDescription = edited.StageDescription;
        entity.BoardTiles       = edited.BoardTiles;
        entity.PlayerSpawnPoint = edited.PlayerSpawnPoint;
        entity.EnemySpawners    = edited.EnemySpawners;

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}