using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RemoteConfig.Application.Enemies.Responses;
using RemoteConfig.Application.Interfaces;

namespace RemoteConfig.Application.Enemies.Queries;

public class GetEnemyListQueryHandler(IRemoteConfigContext dbContext, IMapper mapper) 
    : IRequestHandler<GetEnemiesListQuery, IList<GetEnemyResponse>>
{
    public async Task<IList<GetEnemyResponse>> Handle(GetEnemiesListQuery request, CancellationToken cancellationToken)
    {
        var list = await dbContext.Enemies
            .ToListAsync(cancellationToken);
        
        return mapper.Map<IList<GetEnemyResponse>>(list);    
    }
}