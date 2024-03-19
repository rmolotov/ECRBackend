using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RemoteConfig.Application.Heroes.Responses;
using RemoteConfig.Application.Interfaces;

namespace RemoteConfig.Application.Heroes.Queries;

public class GetNHeroesListQueryHandler(IRemoteConfigContext dbContext, IMapper mapper) 
    : IRequestHandler<GetHeroesListQuery, IList<GetHeroResponse>>
{
    public async Task<IList<GetHeroResponse>> Handle(GetHeroesListQuery request, CancellationToken cancellationToken)
    {
        var list = await dbContext.Heroes
            .ProjectTo<GetHeroResponse>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return list;
    }
}