using MediatR;
using RemoteConfig.Application.Common.Exceptions;
using RemoteConfig.Application.Interfaces;
using RemoteConfig.Core.Entities.Stage;

namespace RemoteConfig.Application.Stages.Commands.DeleteStage;

public class DeleteStageCommandHandler(IRemoteConfigContext dbContext) : IRequestHandler<DeleteStageCommand>
{
    public async Task Handle(DeleteStageCommand request, CancellationToken cancellationToken)
    {
        var stage = await dbContext.Stages.FindAsync(
            [request.Id],
            cancellationToken);

        if (stage == null)
            throw new NotFoundException(nameof(Stage), request.Id);

        dbContext.Stages.Remove(stage);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}