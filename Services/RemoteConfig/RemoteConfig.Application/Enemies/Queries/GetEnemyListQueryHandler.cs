using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RemoteConfig.Application.Enemies.Responses;
using RemoteConfig.Application.Interfaces;

namespace RemoteConfig.Application.Enemies.Queries;

public class GetEnemyListQueryHandler(IRemoteConfigContext dbContext, IMapper mapper, ICacheService cacheService) 
    : IRequestHandler<GetEnemiesListQuery, IList<GetEnemyResponse>>
{
    public async Task<IList<GetEnemyResponse>> Handle(GetEnemiesListQuery request, CancellationToken cancellationToken) =>
        await cacheService.GetAsync(
            key: "Enemies",
            async () =>
            {
                var list = await dbContext.Enemies
                    .ToListAsync(cancellationToken);
        
                return mapper.Map<IList<GetEnemyResponse>>(list);
            },
            token: cancellationToken);
}