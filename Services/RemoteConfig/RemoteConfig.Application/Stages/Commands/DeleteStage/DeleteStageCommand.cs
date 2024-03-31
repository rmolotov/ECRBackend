using MediatR;

namespace RemoteConfig.Application.Stages.Commands.DeleteStage;

public class DeleteStageCommand : IRequest
{
    public string Id { get; set; }
}