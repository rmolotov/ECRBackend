using MediatR;
using RemoteConfig.Application.Interfaces;
using RemoteConfig.Core.Entities.Stage;

namespace RemoteConfig.Application.Stages.Commands.CreateStage;


public class CreateStageQueryHandler(IRemoteConfigContext dbContext) : IRequestHandler<CreateStageCommand, string>
{
    public async Task<string> Handle(CreateStageCommand request, CancellationToken cancellationToken)
    {
        var stage = new Stage()
        {
            Id = request.Id,
            
            StageTitle = request.StageTitle,
            StageDescription = request.StageDescription,
            
            BoardTiles = request.BoardTiles,
            PlayerSpawnPoint = request.PlayerSpawnPoint,
            EnemySpawners = request.EnemySpawners,
        };

        await dbContext.Stages.AddAsync(stage, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return stage.Id;
    }
}