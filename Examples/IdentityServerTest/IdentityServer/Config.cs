using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer
{
    public static class Config
    {
        // OpenID Connect allowed scopes
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        // APIs to be protected
        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource("ResourceApi", "Example Resource API")
            };
        }

        // Clients allowed to request for tokens
        public static IEnumerable<Client> GetClients()
        {
            string resourceUrl = "http://localhost:5001";
            string mvcClientUrl = "http://localhost:5002";
            string spaClientUrl = "http://localhost:5003";
            return new List<Client>
            {
                // ConsoleApp client
                new Client
                {
                    ClientId = "ConsoleAppClient",
                    ClientName = "ConsoleApp Client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "ResourceApi" },
                },

                // Resource owner password grant client
                new Client
                {
                    ClientId = "ResourceOwnerClient",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "ResourceApi" }
                },
                // MVC client
                new Client
                {
                    ClientId = "MvcClient",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.Hybrid,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    RedirectUris           = { $"{mvcClientUrl}/signin-oidc" },
                    PostLogoutRedirectUris = { $"{mvcClientUrl}/signout-callback-oidc" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "ResourceApi"
                    },

                    AllowOfflineAccess = true
                },
                // Resource API Swagger UI
                new Client
                {
                    ClientId = "resourcesswaggerui",
                    ClientName = "Resource Swagger UI",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { $"{resourceUrl}/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"{resourceUrl}/swagger/" },

                    AllowedScopes =
                    {
                        "ResourceApi"
                    }
                },
                // JavaScript Client
                new Client
                {
                    ClientId = "JsClient",
                    ClientName = "JavaScript Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris =           { $"{spaClientUrl}/callback.html" },
                    PostLogoutRedirectUris = { $"{spaClientUrl}/index.html" },
                    AllowedCorsOrigins =     { $"{spaClientUrl}" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "ResourceApi"
                    }
                }
            };
        }

        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser>()
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "demo",
                    Password = "demo".Sha256()
                }
            };
        }
    }
}
