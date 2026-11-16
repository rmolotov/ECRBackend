namespace Shared.Api.Configurations;

public record IdentitySettings(string Uri)
{
    public const string SectionName = "IdentitySettings";

    public string? Audience { get; init; }
    public string? IssuerSigningKey { get; init; }
    public int ClockSkew { get; init; }
    public string? Scope { get; init; }
    public Dictionary<string, string> ValidIssuers { get; init; } = [];
}
