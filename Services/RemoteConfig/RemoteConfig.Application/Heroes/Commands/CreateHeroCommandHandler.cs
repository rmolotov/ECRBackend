using MediatR;
using RemoteConfig.Application.Interfaces;
using RemoteConfig.Core.Entities.Hero;

namespace RemoteConfig.Application.Heroes.Commands;

public class CreateHeroCommandHandler(IRemoteConfigContext dbContext) : IRequestHandler<CreateHeroCommand, string>
{
    public async Task<string> Handle(CreateHeroCommand request, CancellationToken cancellationToken)
    {
        var hero = new Hero()
        {
            Id = request.Id,
            
            Capacity = request.Capacity,
            Current = request.Current,
            Resistance = request.Resistance,
            Voltage = request.Voltage,
        };

        await dbContext.Heroes.AddAsync(hero, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return hero.Id;
    }
}