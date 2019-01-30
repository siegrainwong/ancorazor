#region

using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

#endregion

namespace BlogIdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new[]
            {
                new ApiResource("restapi", "My RESTful API")
            };
        }

        public static IEnumerable<Client> GetClients(string clientHost)
        {   
            return new[]
            {
                // Angular client using implicit flow
                new Client
                {
                    ClientId = "siegrain-blog-client",
                    ClientName = "Siegrain's blog client",
                    ClientUri = clientHost,

                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false, // 是否需要同意
                    AccessTokenLifetime = 180,  // Access token 有效期，秒

                    RedirectUris =
                    {
                        $"{clientHost}signin-oidc",
                        $"{clientHost}redirect-silentrenew"
                    },

                    PostLogoutRedirectUris = { clientHost },
                    AllowedCorsOrigins = { clientHost },

                    AllowedScopes = {"openid", "profile", "email", "restapi"}
                },

                // MVC client using hybrid flow
                new Client
                {
                    ClientId = "mvcclient",
                    ClientName = "MVC 客户端",

                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                    RedirectUris = { "https://localhost:7001/signin-oidc" },
                    FrontChannelLogoutUri = "https://localhost:7001/signout-oidc",
                    PostLogoutRedirectUris = { "https://localhost:7001/signout-callback-oidc" },

                    AllowOfflineAccess = true, // offline_access
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "restapi"
                    }
                }
            };
        }
    }
}