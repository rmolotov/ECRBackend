using MediatR;
using RemoteConfig.Application.Interfaces;

namespace RemoteConfig.Application.Common.Behaviors;

public class CachingBehavior<TRequest, TResponse>(ICacheService cacheService) 
    : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : ICacheableRequest<TResponse>
    where TResponse : class
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken token)
    {
        var cachedResult = await cacheService.GetAsync<TResponse>(request.CacheKey, token);

        if (cachedResult != null)
            return cachedResult;

        var result = await next();

        await cacheService.SetAsync(
            key: request.CacheKey,
            value: result,
            token: token);

        return result;
    }
}