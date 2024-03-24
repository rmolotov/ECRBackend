using MediatR;
using Microsoft.EntityFrameworkCore;
using RemoteConfig.Application.Common.Exceptions;
using RemoteConfig.Application.Interfaces;
using RemoteConfig.Core.Entities.Hero;

namespace RemoteConfig.Application.Heroes.Commands.UpdateHero;

public class UpdateHeroCommandHandler(IRemoteConfigContext dbContext) : IRequestHandler<UpdateHeroCommand>
{
    public async Task Handle(UpdateHeroCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Heroes.FirstOrDefaultAsync(
            h => h.Id == request.Id, cancellationToken);
        
        if (entity == null)
            throw new NotFoundException(nameof(Hero), request.Id);

        entity.Capacity   = request.Capacity;
        entity.Current    = request.Current;
        entity.Resistance = request.Resistance;
        entity.Voltage    = request.Voltage;
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}