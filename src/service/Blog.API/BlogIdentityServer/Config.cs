// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;
using Blog.Common.Configuration;
using IdentityServer4;

namespace BlogIdentityServer
{
    public static class Config
    {
        private const string ClientHost = "https://localhost:5001";
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

        /// <summary>
        /// Mark: 指定客户端
        /// 放置你需要使用ID4认证的客户端
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                // MVC client using hybrid flow
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",

                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                    RedirectUris = { "https://localhost:7000/signin-oidc" },
                    FrontChannelLogoutUri = "https://localhost:7000/signout-oidc",
                    PostLogoutRedirectUris = { "https://localhost:7000/signout-callback-oidc" },

                    AllowOfflineAccess = true, // offline_access
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "restapi"
                    }
                },

                // Angular client using implicit flow
                new Client
                {
                    ClientId = "siegrain-blog-client",
                    ClientName = "Siegrain's Blog Client",
                    ClientUri = ClientHost,

                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                    AccessTokenLifetime = 180,

                    RedirectUris =
                    {
                        $"{ClientHost}/signin-oidc",
                        $"{ClientHost}/redirect-silentrenew"
                    },

                    PostLogoutRedirectUris = { ClientHost },
                    AllowedCorsOrigins = { ClientHost },

                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "restapi" }
                }
            };
        }
    }
}