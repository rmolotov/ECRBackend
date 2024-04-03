using MediatR;
using RemoteConfig.Core.Entities.Stage;

namespace RemoteConfig.Application.Stages.Commands.CreateStage;

public class CreateStageCommand : IRequest<string>
{
    public string Id { get; set; }
    public string StageTitle { get; set; }
    public string StageDescription { get; set; }

    public List<decimal> PlayerSpawnPoint { get; set; }
    public List<EnemySpawner> EnemySpawners { get; set; }
    public List<int[]> BoardTiles { get; set; }
}