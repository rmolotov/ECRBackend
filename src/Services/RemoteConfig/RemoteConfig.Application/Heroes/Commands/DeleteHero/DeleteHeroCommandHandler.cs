using MediatR;
using RemoteConfig.Application.Common.Exceptions;
using RemoteConfig.Application.Interfaces;
using RemoteConfig.Core.Entities.Hero;

namespace RemoteConfig.Application.Heroes.Commands.DeleteHero;

public class DeleteHeroCommandHandler(IRemoteConfigContext dbContext) : IRequestHandler<DeleteHeroCommand>
{
    public async Task Handle(DeleteHeroCommand request, CancellationToken cancellationToken)
    {
        var hero = await dbContext.Heroes.FindAsync(
            [request.Id],
            cancellationToken);

        if (hero == null)
            throw new NotFoundException(nameof(Hero), request.Id);

        dbContext.Heroes.Remove(hero);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}