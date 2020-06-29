using System.Collections.Generic;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace Identity.API.Configuration
{
    public class Config
    {
        // private const string ClientOrigin = "http://localhost:3000";
        
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource
                {
                    Name = JwtClaimTypes.Role,
                    UserClaims = { JwtClaimTypes.Role }
                }
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("kickstarterGateway", "Kickstarter Gateway"),
                new ApiScope("company", "Company API", new List<string>{ JwtClaimTypes.Role }),
                new ApiScope("funding", "Funding API", new List<string>{ "role" }),
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
                    },
                    RedirectUris = { "https://localhost:3000/auth-callback", "https://localhost:3000/silent_renew.html" },
                    PostLogoutRedirectUris = { "https://localhost:3000/" },
                    AllowedCorsOrigins = { "https://localhost:3000    " },

                    AllowOfflineAccess = true,
                    AllowAccessTokensViaBrowser = true
                }
            };
        }
    }
}