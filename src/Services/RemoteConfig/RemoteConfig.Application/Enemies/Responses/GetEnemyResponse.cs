namespace RemoteConfig.Application.Enemies.Responses;

public class GetEnemyResponse
{
    public string Id { get; set; }
    public string EnemyType { get; set; }
    public string AttackType { get; set; }
    public int Capacity { get; set; }
    public int Current { get; set; }
    public int Voltage { get; set; }
    public int Resistance { get; set; }
}