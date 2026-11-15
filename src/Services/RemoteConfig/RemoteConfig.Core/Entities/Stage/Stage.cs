namespace RemoteConfig.Core.Entities.Stage;

public class Stage : BaseEntity
{
    public string StageTitle { get; set; }
    public string StageDescription { get; set; }

    public int[] PlayerSpawnPoint { get; set; }
    public List<EnemySpawner> EnemySpawners { get; set; }
    public List<BoardTile> BoardTiles { get; set; }
}