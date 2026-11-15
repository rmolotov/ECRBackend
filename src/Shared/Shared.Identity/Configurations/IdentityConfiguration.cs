namespace Shared.Identity.Configurations;

public record IdentityConfiguration(string Uri)
{
    public const string SectionName = "IdentityConfiguration";
    
    public string? Audience { get; init; }
    public string? IssuerSigningKey { get; init; }
    public int ClockSkew { get; init; }
    public List<string> ValidIssuers { get; init; } = [];
}