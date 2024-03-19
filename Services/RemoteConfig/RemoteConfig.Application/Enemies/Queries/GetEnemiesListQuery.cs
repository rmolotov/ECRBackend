using MediatR;
using RemoteConfig.Application.Enemies.Responses;

namespace RemoteConfig.Application.Enemies.Queries;

public class GetEnemiesListQuery : IRequest<IList<GetEnemyResponse>>
{
    
}