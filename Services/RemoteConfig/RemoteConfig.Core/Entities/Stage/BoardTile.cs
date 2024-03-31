namespace RemoteConfig.Core.Entities.Stage;

public class BoardTile
{
    #warning change serialization to custom array int[5]?
    public string Tile { get; set; }
    public int TileRotation { get; set; }
    public int[] Position { get; set; }
}