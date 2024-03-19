using MediatR;
using RemoteConfig.Application.Enemies.Responses;

namespace RemoteConfig.Application.Enemies.Queries;

public class GetEnemyQuery(string id) : IRequest<GetEnemyResponse>
{
    public string Id { get; set; } = id;
}