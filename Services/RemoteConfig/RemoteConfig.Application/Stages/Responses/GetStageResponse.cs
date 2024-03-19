using RemoteConfig.Core.Entities.Stage;

namespace RemoteConfig.Application.Stages.Responses;

public class GetStageResponse
{
    public string Id { get; set; }
    public string StageTitle { get; set; }
    public string StageDescription { get; set; }

    public decimal[] PlayerSpawnPoint { get; set; }
    public EnemySpawner[] EnemySpawners { get; set; }
    public BoardTile[] BoardTiles { get; set; }
}