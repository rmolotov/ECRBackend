namespace RemoteConfig.Application.Heroes.Responses;

public class GetHeroResponse
{
    public string Id { get; set; }
    public int Current { get; set; }
    public int Capacity { get; set; }
    public int Voltage { get; set; }
    public int Resistance { get; set; } 
}