using MediatR;
using RemoteConfig.Application.Stages.Responses;

namespace RemoteConfig.Application.Stages.Queries;

public class GetStagesListQuery : IRequest<IList<GetStageResponse>>
{
    
}