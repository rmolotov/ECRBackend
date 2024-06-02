using RemoteConfig.Application.Enemies.Responses;
using RemoteConfig.Application.Interfaces;

namespace RemoteConfig.Application.Enemies.Queries;

public class GetEnemiesListQuery : ICacheableRequest<IList<GetEnemyResponse>>
{
    public string CacheKey { get; init; } = nameof(GetEnemiesListQuery);
    public DateTime? ExpiredAt { get; init; }
}