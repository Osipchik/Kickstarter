using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;

namespace Identity.API.Configuration
{
    public class Config
    {
        //private const string ClientOrigin = "http://localhost:3000";

        private const string ClientOrigin = "https://www.getpostman.com";

        public static IEnumerable<IdentityResource> GetResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        
        public static IEnumerable<ApiResource> GetApis =>
            new List<ApiResource>
            {
                new ApiResource("identity", "Identity Service"),
                new ApiResource("company", "Company Service"),
                new ApiResource("funding", "Funding Service"),
                new ApiResource("comment", "Comment Service")
            };

        public static IEnumerable<Client> Clients(IConfigurationSection clientIds)
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = clientIds["JsClientId"],
                    ClientName = "Kickstarter React Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowAccessTokensViaBrowser = true,
                    RequirePkce = true,
                    RequireConsent = false,
                    
                    // RedirectUris = { $"{ClientOrigin}/callback" },
                    RedirectUris = { "https://www.getpostman.com/oauth2/callback" },
                    PostLogoutRedirectUris = { ClientOrigin },
                    AllowedCorsOrigins = { ClientOrigin },
                    EnableLocalLogin = true,
                    
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "identity", 
                        "company", 
                        "roles",
                        "funding",
                        "comment"
                    }
                },
                new Client
                {
                    ClientId = "pkce_client",
                    ClientName = "MVC PKCE Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,
                    RedirectUris = {"http://localhost:5001/signin-oidc"},
                    AllowedScopes = {"openid", "profile", "api1"},

                    RequirePkce = true,
                    AllowPlainTextPkce = false,
                    AllowAccessTokensViaBrowser = true
                }
            };
        }
    }
}