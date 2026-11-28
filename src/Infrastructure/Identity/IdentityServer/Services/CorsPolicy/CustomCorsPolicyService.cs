using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IdentityServer.Services.CorsPolicy;

public class CustomCorsPolicyService(IHttpContextAccessor context, ILogger<DefaultCorsPolicyService> logger) 
    : ICorsPolicyService
{
    private const string WILDCARD_SUBDOMAIN = "*.";
    private const string ALLOWED_BY_CONFIG_TEMPLATE = "[{0}]: {1} configured and origin [{2}] is {3}";
    private const string BLOCKED_BY_UNCONFIGURED_TEMPLATE = "[{0}]: Exiting; origin {1} is not allowed";

    public async Task<bool> IsOriginAllowedAsync(string origin)
    {
        origin = origin.ToLowerInvariant();
        var allowedOrigins = await context.HttpContext!.RequestServices
            .GetRequiredService<IConfigurationDbContext>()
            .ClientCorsOrigins
            .ToListAsync();

        if (allowedOrigins.Any(clientCorsOrigin => clientCorsOrigin.Origin == origin))
        {
            logger.LogDebug(
                string.Format(ALLOWED_BY_CONFIG_TEMPLATE,
                    nameof(CustomCorsPolicyService),
                    nameof(allowedOrigins),
                    origin,
                    "allowed"));

            return true;
        }

        if (Uri.TryCreate(origin, UriKind.Absolute, out var originUri) == false)
        {
            logger.LogDebug(
                string.Format(BLOCKED_BY_UNCONFIGURED_TEMPLATE,
                    nameof(CustomCorsPolicyService),
                    origin));

            return false;
        }

        var allowed = allowedOrigins
            .Where(clientCorsOrigin => clientCorsOrigin.Origin.Contains($"://{WILDCARD_SUBDOMAIN}"))
            .Select(clientCorsOrigin => CorsPolicyExtensions.CreateDomainUri(clientCorsOrigin.Origin))
            .Any(domain => CorsPolicyExtensions.IsSubdomainOf(originUri, domain));

        logger.LogDebug(
            string.Format(ALLOWED_BY_CONFIG_TEMPLATE,
                nameof(CustomCorsPolicyService),
                nameof(allowedOrigins),
                origin,
                allowed ? "allowed" : "not allowed"));

        return allowed;
    }
}