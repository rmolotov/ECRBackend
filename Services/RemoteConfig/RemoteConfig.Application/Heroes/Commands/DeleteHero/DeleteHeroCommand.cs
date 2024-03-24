using MediatR;

namespace RemoteConfig.Application.Heroes.Commands.DeleteHero;

public class DeleteHeroCommand : IRequest
{
    public string Id { get; set; }
}