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

        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
                new ApiResource("kickstarterGateway", "kickstarterGateway API")
                {
                    Scopes  = {"kickstarterGateway"}
                },
                new ApiResource("company", "Company API", new List<string>{ JwtClaimTypes.Role })
                {
                    Scopes = {"company", "kickstarterGateway"}
                },
                new ApiResource("funding", "Funding API", new List<string>{ JwtClaimTypes.Role })
                {
                    Scopes = {"funding"}
                }
                // new ApiResource("kickstarterGateway"),
                // new ApiResource("company", new List<string>{ JwtClaimTypes.Role }),
                // new ApiResource("funding", new List<string>{ JwtClaimTypes.Role }),
                // new ApiResource("updates", new List<string>{ JwtClaimTypes.Role }),
            };
        
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("kickstarterGateway"),
                new ApiScope("company", new List<string>{ JwtClaimTypes.Role }),
                new ApiScope("funding", new List<string>{ JwtClaimTypes.Role }),
                new ApiScope("updates", new List<string>{ JwtClaimTypes.Role }),
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
                        "updates",
                    },
                    RedirectUris = { "https://localhost:3000/auth-callback", "https://localhost:3000/silent_renew.html" },
                    PostLogoutRedirectUris = { "https://localhost:3000/" },
                    AllowedCorsOrigins = { "https://localhost:3000" },

                    AllowOfflineAccess = true,
                    AllowAccessTokensViaBrowser = true
                }
            };
        }
    }
}