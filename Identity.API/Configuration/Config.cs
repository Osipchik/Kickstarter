using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace Identity.API.Configuration
{
    public class Config
    {
        // private const string ClientOrigin = "http://localhost:3000";
        
        public static IEnumerable<IdentityResource> GetResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource("roles", new[] { "role" })
            };
        
        public static IEnumerable<ApiResource> GetApis =>
            new List<ApiResource>
            {
                new ApiResource("kickstarterGateway", "Kickstarter Gateway"),
                new ApiResource("company", "Company API", new List<string>{"role"}),
                new ApiResource("funding", "Funding API", new List<string>{"role"}),
            };
        
//clientIds["JsClientId"] IConfigurationSection clientIds
        public static IEnumerable<Client> Clients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "jsClient",
                    
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireConsent = false,
                    RequirePkce = true,
                    AllowPlainTextPkce = false,
                    RequireClientSecret = false,
                    
                    AlwaysSendClientClaims = true,
                    AlwaysIncludeUserClaimsInIdToken = true,

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "kickstarterGateway",
                        "company",
                        "funding",
                        "roles",
                    },
                    RedirectUris = { "https://localhost:3000/auth-callback", "https://localhost:3000/silent_renew.html" },
                    PostLogoutRedirectUris = { "https://localhost:3000/" },

                    AllowOfflineAccess = true,
                    AllowAccessTokensViaBrowser = true
                }
            };
        }
    }
}