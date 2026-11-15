using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RemoteConfig.Application.Interfaces;
using RemoteConfig.Application.Stages.Responses;

namespace RemoteConfig.Application.Stages.Queries;

public class GetStagesListQueryHandler(IRemoteConfigContext dbContext, IMapper mapper) 
    : IRequestHandler<GetStagesListQuery, IList<GetStageResponse>>
{
    public async Task<IList<GetStageResponse>> Handle(GetStagesListQuery request, CancellationToken cancellationToken)
    {
        var list = await dbContext.Stages
            .ToListAsync(cancellationToken);

        return mapper.Map<IList<GetStageResponse>>(list);
    }
}