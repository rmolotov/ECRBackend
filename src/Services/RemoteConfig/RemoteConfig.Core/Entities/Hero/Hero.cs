namespace RemoteConfig.Core.Entities.Hero;

public class Hero : BaseEntity
{
    public int Current { get; set; }
    public int Capacity { get; set; }
    public int Voltage { get; set; }
    public int Resistance { get; set; }   
}