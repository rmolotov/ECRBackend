namespace RemoteConfig.Core.Entities.Enemy;

public class Enemy : BaseEntity
{
    public EnemyType EnemyType { get; set; }
    public AttackType AttackType { get; set; }
    public int Capacity { get; set; }
    public int Current { get; set; }
    public int Voltage { get; set; }
    public int Resistance { get; set; }
}