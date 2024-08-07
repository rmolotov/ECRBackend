using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RemoteConfig.Application.Common.Behaviors;

namespace RemoteConfig.Application.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services) =>
        services
            .AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())

            .AddTransient(
                typeof(IPipelineBehavior<,>),
                typeof(LoggingBehavior<,>))
            .AddTransient(
                typeof(IPipelineBehavior<,>),
                typeof(ValidatorBehavior<,>))
            .AddTransient(
                typeof(IPipelineBehavior<,>), 
                typeof(CachingBehavior<,>));
}