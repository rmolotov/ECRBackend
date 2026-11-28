using MediatR;
using RemoteConfig.Application.Heroes.Responses;

namespace RemoteConfig.Application.Heroes.Queries;

public class GetHeroesListQuery : IRequest<IList<GetHeroResponse>>
{
    
}