using MediatR;

namespace RemoteConfig.Application.Heroes.Commands.CreateHero;

public class CreateHeroCommand : IRequest<string>
{
    public string Id { get; set; }
    public int Current { get; set; }
    public int Capacity { get; set; }
    public int Voltage { get; set; }
    public int Resistance { get; set; } 
}