using AutoMapper;
using MediatR;
using RemoteConfig.Application.Interfaces;
using RemoteConfig.Core.Entities.Stage;

namespace RemoteConfig.Application.Stages.Commands.CreateStage;


public class CreateStageQueryHandler(IRemoteConfigContext dbContext, IMapper mapper) : IRequestHandler<CreateStageCommand, string>
{
    public async Task<string> Handle(CreateStageCommand request, CancellationToken cancellationToken)
    {
        var stage = mapper.Map<Stage>(request);

        await dbContext.Stages.AddAsync(stage, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return stage.Id;
    }
}