using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;

namespace Identity.API.Configuration
{
    public class Config
    {
        // private const string ClientOrigin = "http://localhost:3000";

        // private const string ClientOrigin = "https://www.getpostman.com";

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
//clientIds["JsClientId"]
        public static IEnumerable<Client> Clients(IConfigurationSection clientIds)
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "jsClient",
                    ClientName = "Kickstarter React Client",
                    
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,

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
                    },
                    RedirectUris = { "https://localhost:3000/auth-callback", "https://localhost:3000/silent_renew.html" },
                    PostLogoutRedirectUris = { "https://localhost:3000/" },

                    AllowedCorsOrigins = { "https://localhost:3000" },
                    AllowAccessTokensViaBrowser = true,
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