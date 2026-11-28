using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RemoteConfig.Application.Common.Exceptions;
using RemoteConfig.Application.Heroes.Responses;
using RemoteConfig.Application.Interfaces;
using RemoteConfig.Core.Entities.Hero;

namespace RemoteConfig.Application.Heroes.Queries;

public class GetHeroQueryHandler(IRemoteConfigContext dbContext, IMapper mapper)
    : IRequestHandler<GetHeroQuery, GetHeroResponse>
{
    public async Task<GetHeroResponse> Handle(GetHeroQuery request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Heroes.FirstOrDefaultAsync(
            e => e.Id == request.Id, 
            cancellationToken);

        if (entity == null)
            throw new NotFoundException(nameof(Hero), request.Id);

        return mapper.Map<GetHeroResponse>(entity);
    }
}