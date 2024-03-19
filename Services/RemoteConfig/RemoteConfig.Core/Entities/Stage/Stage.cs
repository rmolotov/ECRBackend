namespace RemoteConfig.Core.Entities.Stage;

public class Stage : BaseEntity
{
    public string StageTitle { get; set; }
    public string StageDescription { get; set; }

    public decimal[] PlayerSpawnPoint { get; set; }
    public EnemySpawner[] EnemySpawners { get; set; }
    public BoardTile[] BoardTiles { get; set; }
}