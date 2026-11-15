using System.Collections.Generic;
using IdentityServer4.Models;

namespace IdentityServer.Configuration;

public record IdentityServerConfiguration
{
    public const string SectionName = "IdentityServerConfiguration";
    
    public string TokensSigningKey { get; init; }
    public string Uri { get; init; }
    public string ApiSecret { get; init; }
    public string DevSecret { get; init; }
    public string HandshakeSecret { get; init; }
    
    public IEnumerable<string> IdentityResources { get; init; }
    public IEnumerable<string> ApiScopes { get; init; }
    public IEnumerable<Client> Clients { get; init; }
}