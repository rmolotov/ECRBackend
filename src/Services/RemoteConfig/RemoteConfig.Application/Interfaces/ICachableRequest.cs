using MediatR;

namespace RemoteConfig.Application.Interfaces;

public interface ICacheableRequest<TResponse> : IRequest<TResponse> , ICacheableRequest
    where TResponse : class;

public interface ICacheableRequest
{
    public string CacheKey { get; init; }
    public DateTime? ExpiredAt { get; init; }
}