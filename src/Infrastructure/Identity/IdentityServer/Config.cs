// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using System.Linq;
using IdentityServer.Configuration;
using IdentityServer4;
using IdentityServer4.Models;

namespace Identity;

public static class Config
{
    public const string ApiScope = "ECR.Api";
    public const string WebScope = "ECR.Web";

    public static IEnumerable<IdentityResource> GetIdentityResources(IdentityServerConfiguration configuration) =>
        configuration.IdentityResources
            .Select(ConfigurationExtensions.CreateStandardIdentityResource);

    public static IEnumerable<ApiScope> GetApiScopes(IdentityServerConfiguration configuration) =>
        configuration.ApiScopes
            .Select(ConfigurationExtensions.CreateApiScope);

    public static IEnumerable<ApiResource> GetApiResources(IdentityServerConfiguration configuration) =>
        configuration.ApiResources
            .Select(resource => new ApiResource(resource.Name)
            {
                Scopes = resource.Scopes.ToList()
            });

    public static IEnumerable<Client> GetClients(IdentityServerConfiguration configuration) =>
        new Client[]
        {
            new()
            {
                ClientId = "Client.Api",
                ClientName = "API client",

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { ConfigurationExtensions.CreateClientSecret(configuration.ApiSecret) },

                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    ApiScope,
                    WebScope
                }
            },

            new()
            {
                ClientId = "Client.Dev",
                ClientName = "Developer client",

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { ConfigurationExtensions.CreateClientSecret(configuration.DevSecret) },

                AllowedScopes =
                {
                    ApiScope,
                    WebScope
                }
            },

            new()
            {
                ClientId = "Client.Web",
                ClientName = "Web Client",

                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                RequireClientSecret = false,
                AllowOfflineAccess = true,

                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    WebScope
                }
            },
        };
}
