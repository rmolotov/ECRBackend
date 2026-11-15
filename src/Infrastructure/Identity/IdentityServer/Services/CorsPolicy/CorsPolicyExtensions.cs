using System;

namespace IdentityServer.Services.CorsPolicy;

public static class CorsPolicyExtensions
{
    private const string WILDCARD_SUBDOMAIN = "*.";
    
    public static Uri CreateDomainUri(string origin) =>
        new(
            origin.Replace(WILDCARD_SUBDOMAIN, string.Empty),
            UriKind.Absolute);
    
    public static bool IsSubdomainOf(Uri subdomain, Uri domain) =>
        subdomain.IsAbsoluteUri
        && domain.IsAbsoluteUri
        && subdomain.Scheme == domain.Scheme
        && subdomain.Port == domain.Port
        && subdomain.Host.EndsWith($".{domain.Host}", StringComparison.Ordinal);
}