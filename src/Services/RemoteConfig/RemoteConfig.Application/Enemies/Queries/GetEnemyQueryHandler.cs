using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RemoteConfig.Application.Common.Exceptions;
using RemoteConfig.Application.Enemies.Responses;
using RemoteConfig.Application.Interfaces;
using RemoteConfig.Core.Entities.Enemy;

namespace RemoteConfig.Application.Enemies.Queries;

public class GetEnemyQueryHandler(IRemoteConfigContext dbContext, IMapper mapper) : IRequestHandler<GetEnemyQuery, GetEnemyResponse>
{
    public async Task<GetEnemyResponse> Handle(GetEnemyQuery request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Enemies.FirstOrDefaultAsync(
            e => e.Id == request.Id, 
            cancellationToken);

        if (entity == null)
            throw new NotFoundException(nameof(Enemy), request.Id);

        return mapper.Map<GetEnemyResponse>(entity);
    }
}