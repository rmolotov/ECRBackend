using System;
using IdentityServer4.Models;

namespace IdentityServer.Configuration;

public static class ConfigurationExtensions
{
    public static ApiScope CreateApiScope(string name) => 
        new(name);
    
    public static IdentityResource CreateStandardIdentityResource(string standardScopeName) =>
        standardScopeName switch
        {
            "OpenId" => new IdentityResources.OpenId(),
            "Profile" => new IdentityResources.Profile(),
            "Email" => new IdentityResources.Email(),
            "Phone" => new IdentityResources.Phone(),
            "Address" => new IdentityResources.Address(),
            _ => throw new ArgumentOutOfRangeException(nameof(standardScopeName), standardScopeName, null)
        };

    public static Secret CreateClientSecret(string secretInput) =>
        new Secret(secretInput.Sha256());
}