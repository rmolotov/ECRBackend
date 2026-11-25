using System.Collections.Generic;

namespace IdentityServer.Configuration;

public record IdentityServerConfiguration
{
    public const string WellKnownName = "IdentityServerConfiguration";

    public string IssuerUri { get; init; }

    public HashSet<string> IdentityResources { get; init; } = [];

    public HashSet<string> ApiScopes { get; init; } = [];
    public List<ApiResourceConfiguration> ApiResources { get; init; } = [];

    public required string ApiSecret { get; init; }
    public required string DevSecret { get; init; }
    public required string IssuerSigningKey { get; init; }
}

public record ApiResourceConfiguration
{
    public string Name { get; init; }
    public HashSet<string> Scopes { get; init; } = [];
}
