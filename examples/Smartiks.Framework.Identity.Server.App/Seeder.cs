using IdentityModel;
using IdentityServer4;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Smartiks.Framework.Identity.Data;
using Smartiks.Framework.Identity.Data.Abstractions;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Smartiks.Framework.Identity.Server.App
{
    public class Seeder
    {
        private IdentityDataContext<int, User<int>, Role<int>> context;

        private UserManager<User<int>> userManager;

        public Seeder(IdentityDataContext<int, User<int>, Role<int>> context, UserManager<User<int>> userManager)
        {
            this.context = context;

            this.userManager = userManager;
        }

        public async Task SeedAsync()
        {
            var identityResources = new IdentityResource[] {
                new IdentityResources.OpenId(),
                //new IdentityResources.Profile(),
                //new IdentityResources.Email(),
                //new IdentityResources.Phone(),
                //new IdentityResources.Address()
            };

            var apiResources = new[] {
                new ApiResource {
                    Name = "api",
                    DisplayName = "API",
                    Description = "API",
                    ApiSecrets = {
                        new Secret("7c47d19600af41c697fb697dd7d00fa5", "App Master Secret"),
                        new Secret {
                            Type = IdentityServerConstants.SecretTypes.X509CertificateBase64,
                            Value = "MIIDATCCAe2gAwIBAgIQoHUYAquk9rBJcq8W+F0FAzAJBgUrDgMCHQUAMBIxEDAOBgNVBAMTB0RldlJvb3QwHhcNMTAwMTIwMjMwMDAwWhcNMjAwMTIwMjMwMDAwWjARMQ8wDQYDVQQDEwZDbGllbnQwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQDSaY4x1eXqjHF1iXQcF3pbFrIbmNw19w/IdOQxbavmuPbhY7jX0IORu/GQiHjmhqWt8F4G7KGLhXLC1j7rXdDmxXRyVJBZBTEaSYukuX7zGeUXscdpgODLQVay/0hUGz54aDZPAhtBHaYbog+yH10sCXgV1Mxtzx3dGelA6pPwiAmXwFxjJ1HGsS/hdbt+vgXhdlzud3ZSfyI/TJAnFeKxsmbJUyqMfoBl1zFKG4MOvgHhBjekp+r8gYNGknMYu9JDFr1ue0wylaw9UwG8ZXAkYmYbn2wN/CpJl3gJgX42/9g87uLvtVAmz5L+rZQTlS1ibv54ScR2lcRpGQiQav/LAgMBAAGjXDBaMBMGA1UdJQQMMAoGCCsGAQUFBwMCMEMGA1UdAQQ8MDqAENIWANpX5DZ3bX3WvoDfy0GhFDASMRAwDgYDVQQDEwdEZXZSb290ghAsWTt7E82DjU1E1p427Qj2MAkGBSsOAwIdBQADggEBADLje0qbqGVPaZHINLn+WSM2czZk0b5NG80btp7arjgDYoWBIe2TSOkkApTRhLPfmZTsaiI3Ro/64q+Dk3z3Kt7w+grHqu5nYhsn7xQFAQUf3y2KcJnRdIEk0jrLM4vgIzYdXsoC6YO+9QnlkNqcN36Y8IpSVSTda6gRKvGXiAhu42e2Qey/WNMFOL+YzMXGt/nDHL/qRKsuXBOarIb++43DV3YnxGTx22llhOnPpuZ9/gnNY7KLjODaiEciKhaKqt/b57mTEz4jTF4kIg6BP03MUfDXeVlM1Qf1jB43G2QQ19n5lUiqTpmQkcfLfyci2uBZ8BkOhXr3Vk9HIk/xBXQ="
                        }
                    },
                    UserClaims = {
                        JwtClaimTypes.Name,
                        JwtClaimTypes.GivenName,
                        JwtClaimTypes.FamilyName,
                        JwtClaimTypes.Email,
                        JwtClaimTypes.EmailVerified
                    },
                    Scopes = { "api.full_access", "api.read_only" }
                }
            };


            var apiScopes = new []
            {
                new ApiScope
                {
                    Name = "api.full_access",
                    DisplayName = "Full Access",
                    Description = "Full Access to API"
                },
                new ApiScope
                {
                    Name = "api.read_only",
                    DisplayName = "Read-Only Access",
                    Description = "Read-Only Access to API"
                }
            };


            var clients = new[] {
                new Client {
                    ClientId = "client.reference",
                    ClientName = "Resource Owner Password (Reference)",
                    ClientSecrets = {
                        new Secret("a3c1210604414881b85064b172b28265", "Client Master Secret")
                    },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        "api.full_access"
                    },
                    AccessTokenType = AccessTokenType.Reference
                },
                new Client {
                    ClientId = "client.rop",
                    ClientName = "Resource Owner Password",
                    ClientSecrets = {
                        new Secret("a3c1210604414881b85064b172b28265", "Client Master Secret")
                    },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        "api.full_access"
                    },
                    AllowOfflineAccess = true
                },
                new Client {
                    ClientId = "client.rop.public",
                    ClientName = "Resource Owner Password (Public)",
                    RequireClientSecret = false,
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        "api.full_access"
                    }
                },
                new Client {
                    ClientId = "client.cc",
                    ClientName = "Client Credentials",
                    ClientSecrets = {
                        new Secret("a3c1210604414881b85064b172b28265", "Client Master Secret")
                    },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        "api.full_access"
                    }
                },
                new Client {
                    ClientId = "client.cc.public",
                    ClientName = "Client Credentials (Public)",
                    RequireClientSecret = false,
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        "api.full_access"
                    }
                },
                new Client {
                    ClientId = "client.implicit",
                    ClientName = "Implicit",
                    ClientUri = "https://www.smartiks.com.tr",
                    LogoUri = "https://media.licdn.com/dms/image/C4E0BAQES3PtJPELJwg/company-logo_200_200/0?e=2159024400&v=beta&t=x0iiTxBt7yV9xPb0LKqzJKaT-_snRUUqDpX_-Wq83sU",
                    RequireClientSecret = false,
                    AllowAccessTokensViaBrowser = true,
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        "api.full_access"
                    },
                    AllowedCorsOrigins = {
                        "http://localhost:5001",
                        "https://localhost:5001",
                        "http://localhost:5002",
                        "https://localhost:5002"
                    },
                    RedirectUris = {
                        "http://localhost:5001",
                        "https://localhost:5001",
                        "http://localhost:5002",
                        "https://localhost:5002"
                    },
                    PostLogoutRedirectUris = {
                        "http://localhost:5001",
                        "https://localhost:5001",
                        "http://localhost:5002",
                        "https://localhost:5002"
                    }
                },
                new Client {
                    ClientId = "client.mvc",
                    ClientName = "Hybrid",
                    ClientUri = "https://www.smartiks.com.tr",
                    LogoUri = "https://media.licdn.com/dms/image/C4E0BAQES3PtJPELJwg/company-logo_200_200/0?e=2159024400&v=beta&t=x0iiTxBt7yV9xPb0LKqzJKaT-_snRUUqDpX_-Wq83sU",
                    ClientSecrets = {
                        new Secret("a3c1210604414881b85064b172b28265", "Client Master Secret")
                    },
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RedirectUris = { "http://localhost:5001/signin-oidc" },
                    FrontChannelLogoutUri = "http://localhost:5001/signout-oidc",
                    PostLogoutRedirectUris = { "http://localhost:5001/signout-callback-oidc" },
                    AllowOfflineAccess = true,
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        "api.full_access"
                    },
                },
                new Client {
                    ClientId = "client.hybrid.backchannel",
                    ClientName = "Hybrid (BackChannel)",
                    ClientUri = "https://www.smartiks.com.tr",
                    LogoUri = "https://media.licdn.com/dms/image/C4E0BAQES3PtJPELJwg/company-logo_200_200/0?e=2159024400&v=beta&t=x0iiTxBt7yV9xPb0LKqzJKaT-_snRUUqDpX_-Wq83sU",
                    ClientSecrets = {
                        new Secret("a3c1210604414881b85064b172b28265", "Client Master Secret")
                    },
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RedirectUris = { "http://localhost:5001/signin-oidc" },
                    BackChannelLogoutUri = "http://localhost:5001/logout",
                    PostLogoutRedirectUris = { "http://localhost:5001/signout-callback-oidc" },
                    AllowOfflineAccess = true,
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        "api.full_access"
                    },
                }
            };

            foreach (var identityResource in identityResources)
            {
                context.IdentityResources.Add(identityResource.ToEntity());
            }

            foreach (var apiResource in apiResources)
            {
                context.ApiResources.Add(apiResource.ToEntity());
            }

            foreach (var client in clients)
            {
                context.Clients.Add(client.ToEntity());
            }

            foreach (var apiScope in apiScopes)
            {
                context.ApiScopes.Add(apiScope.ToEntity());
            }

            await context.SaveChangesAsync();


            var user = new User<int>
            {
                UserName = "murat.atay",
                Email = "murat.atay@yopmail.com",
                EmailConfirmed = false,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };

            var result = await userManager.CreateAsync(user, "@ZHmc57");

            if (!result.Succeeded)
                throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => $"{e.Code} {e.Description}").ToArray()));

            result = await userManager.AddClaimsAsync(user, new[] {
                new Claim(JwtClaimTypes.Name, "Murat Atay"),
                new Claim(JwtClaimTypes.GivenName, "Murat"),
                new Claim(JwtClaimTypes.FamilyName, "Atay"),
                new Claim(JwtClaimTypes.Email, user.Email),
                new Claim(JwtClaimTypes.EmailVerified, user.EmailConfirmed.ToString(), ClaimValueTypes.Boolean)
            });

            if (!result.Succeeded)
                throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => $"{e.Code} {e.Description}").ToArray()));
        }
    }
}
