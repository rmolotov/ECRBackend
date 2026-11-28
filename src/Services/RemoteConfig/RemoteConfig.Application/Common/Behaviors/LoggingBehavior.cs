using MediatR;
using RemoteConfig.Application.Interfaces;
using Serilog;

namespace RemoteConfig.Application.Common.Behaviors;

public class LoggingBehavior<TRequest, TResponse> (ICurrentUserService currentUserService)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        Log.Information(
            "Notes Request: {Name} {@UserId} {@Request}",
            typeof(TRequest).Name,
            currentUserService.UserId,
            request);

        var response = await next();

        return response;
    }
}