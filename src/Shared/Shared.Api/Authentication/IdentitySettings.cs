namespace Shared.Api.Authentication;

public record IdentitySettings
{
    public const string WellKnownName = "IdentitySettings";

    public required string Authority { get; init; }
    public HashSet<string> ValidAudiences { get; init; } = [];
    public HashSet<string> ValidIssuers { get; init; } = [];
    public int ClockSkew { get; init; }

    public required string IssuerSigningKey { get; init; }
}
