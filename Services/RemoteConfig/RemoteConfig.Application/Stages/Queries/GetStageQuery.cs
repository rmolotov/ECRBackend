using MediatR;
using RemoteConfig.Application.Stages.Responses;

namespace RemoteConfig.Application.Stages.Queries;

public class GetStageQuery(string id) : IRequest<GetStageResponse>
{
    public string Id { get; set; } = id;
}