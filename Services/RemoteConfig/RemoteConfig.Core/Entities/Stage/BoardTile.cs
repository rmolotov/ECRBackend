namespace RemoteConfig.Core.Entities.Stage;

public class BoardTile
{
    public BoardTileType TileType { get; set; }
    public BoardTileRotation TileRotation { get; set; }
    public int[] Position { get; set; }
}