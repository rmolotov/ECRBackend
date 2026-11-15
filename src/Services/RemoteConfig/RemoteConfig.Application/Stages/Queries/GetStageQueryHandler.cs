using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RemoteConfig.Application.Common.Exceptions;
using RemoteConfig.Application.Interfaces;
using RemoteConfig.Application.Stages.Responses;
using RemoteConfig.Core.Entities.Stage;

namespace RemoteConfig.Application.Stages.Queries;

public class GetStageQueryHandler(IRemoteConfigContext dbContext, IMapper mapper) : IRequestHandler<GetStageQuery, GetStageResponse>
{
    public async Task<GetStageResponse> Handle(GetStageQuery request, CancellationToken cancellationToken)
    {
        var stage = await dbContext.Stages.FirstOrDefaultAsync(
            s => s.Id == request.Id,
            cancellationToken);

        if (stage == null)
            throw new NotFoundException(nameof(Stage), request.Id);

        return mapper.Map<GetStageResponse>(stage);
    }
}