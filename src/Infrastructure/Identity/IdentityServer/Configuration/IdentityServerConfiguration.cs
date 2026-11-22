using System.Collections.Generic;

namespace IdentityServer.Configuration;

public record IdentityServerConfiguration
{
    public const string WellKnownName = "IdentityServerConfiguration";

    public string Uri { get; init; }
    public string ApiSecret { get; init; }
    public string DevSecret { get; init; }

    public HashSet<string> IdentityResources { get; init; } = [];
    
    public string TokensSigningKey { get; init; }
}