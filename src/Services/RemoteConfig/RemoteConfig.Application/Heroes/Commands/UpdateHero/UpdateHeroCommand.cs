using MediatR;

namespace RemoteConfig.Application.Heroes.Commands.UpdateHero;

public class UpdateHeroCommand: IRequest
{
    public string Id { get; set; }
    public int Current { get; set; }
    public int Capacity { get; set; }
    public int Voltage { get; set; }
    public int Resistance { get; set; }
}