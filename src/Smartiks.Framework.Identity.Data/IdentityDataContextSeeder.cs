using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Smartiks.Framework.Identity.Data.Abstractions;

namespace Smartiks.Framework.Identity.Data
{
    public class IdentityDataContextSeeder<TId, TUser, TRole>
        where TId : IEquatable<TId>
        where TUser : User<TId>
        where TRole : Role<TId>
    {
        private static IEnumerable<TUser> GetUsers()
        {
            return new [] {
                (TUser) new User<TId> {
                    UserName = "username",
                    NormalizedUserName = "USERNAME",
                    Email = "username@local",
                    NormalizedEmail = "USERNAME@LOCAL"
                }
            };
        }

        private static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[] {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };
        }

        private static IEnumerable<ApiResource> GetApis()
        {
            return new [] {
                new ApiResource("api-1", "API 1") {
                    ApiSecrets = { new Secret("secret".Sha256()) }
                },
                new ApiResource {
                    Name = "api-2",
                    ApiSecrets = {
                        new Secret("secret".Sha256())
                    },
                    UserClaims = {
                        JwtClaimTypes.Name,
                        JwtClaimTypes.Email
                    },
                    Scopes = {
                        new Scope {
                            Name = "api-2.full_access",
                            DisplayName = "Full access to API 2"
                        },
                        new Scope {
                            Name = "api-2.read_only",
                            DisplayName = "Read only access to API 2"
                        }
                    }
                }
            };
        }

        private static IEnumerable<Client> GetClients()
        {
            return new [] {
                ///////////////////////////////////////////
                // Console Client Credentials Flow Sample
                //////////////////////////////////////////
                new Client {
                    ClientId = "client",
                    ClientSecrets = {
                        new Secret("secret".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "api-1", "api-2:read_only" }
                },

                ///////////////////////////////////////////
                // Console Client Credentials Flow with client JWT assertion
                //////////////////////////////////////////
                new Client {
                    ClientId = "client.jwt",
                    ClientSecrets = {
                        new Secret {
                            Type = IdentityServerConstants.SecretTypes.X509CertificateBase64,
                            Value = "MIIDATCCAe2gAwIBAgIQoHUYAquk9rBJcq8W+F0FAzAJBgUrDgMCHQUAMBIxEDAOBgNVBAMTB0RldlJvb3QwHhcNMTAwMTIwMjMwMDAwWhcNMjAwMTIwMjMwMDAwWjARMQ8wDQYDVQQDEwZDbGllbnQwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQDSaY4x1eXqjHF1iXQcF3pbFrIbmNw19w/IdOQxbavmuPbhY7jX0IORu/GQiHjmhqWt8F4G7KGLhXLC1j7rXdDmxXRyVJBZBTEaSYukuX7zGeUXscdpgODLQVay/0hUGz54aDZPAhtBHaYbog+yH10sCXgV1Mxtzx3dGelA6pPwiAmXwFxjJ1HGsS/hdbt+vgXhdlzud3ZSfyI/TJAnFeKxsmbJUyqMfoBl1zFKG4MOvgHhBjekp+r8gYNGknMYu9JDFr1ue0wylaw9UwG8ZXAkYmYbn2wN/CpJl3gJgX42/9g87uLvtVAmz5L+rZQTlS1ibv54ScR2lcRpGQiQav/LAgMBAAGjXDBaMBMGA1UdJQQMMAoGCCsGAQUFBwMCMEMGA1UdAQQ8MDqAENIWANpX5DZ3bX3WvoDfy0GhFDASMRAwDgYDVQQDEwdEZXZSb290ghAsWTt7E82DjU1E1p427Qj2MAkGBSsOAwIdBQADggEBADLje0qbqGVPaZHINLn+WSM2czZk0b5NG80btp7arjgDYoWBIe2TSOkkApTRhLPfmZTsaiI3Ro/64q+Dk3z3Kt7w+grHqu5nYhsn7xQFAQUf3y2KcJnRdIEk0jrLM4vgIzYdXsoC6YO+9QnlkNqcN36Y8IpSVSTda6gRKvGXiAhu42e2Qey/WNMFOL+YzMXGt/nDHL/qRKsuXBOarIb++43DV3YnxGTx22llhOnPpuZ9/gnNY7KLjODaiEciKhaKqt/b57mTEz4jTF4kIg6BP03MUfDXeVlM1Qf1jB43G2QQ19n5lUiqTpmQkcfLfyci2uBZ8BkOhXr3Vk9HIk/xBXQ="
                        }
                    },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "api-1", "api-2:read_only" }
                },

                ///////////////////////////////////////////
                // Console Resource Owner Flow Sample
                //////////////////////////////////////////
                new Client {
                    ClientId = "roclient",
                    ClientSecrets = {
                        new Secret("secret".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowOfflineAccess = true,
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        "api-1", "api-2:read_only"
                    }
                },

                ///////////////////////////////////////////
                // Console Public Resource Owner Flow Sample
                //////////////////////////////////////////
                new Client {
                    ClientId = "roclient.public",
                    RequireClientSecret = false,
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowOfflineAccess = true,
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email,
                        "api-1", "api-2:read_only"
                    }
                },

                ///////////////////////////////////////////
                // Console Hybrid with PKCE Sample
                //////////////////////////////////////////
                new Client {
                    ClientId = "console.hybrid.pkce",
                    ClientName = "Console Hybrid with PKCE Sample",
                    RequireClientSecret = false,
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RequirePkce = true,
                    RedirectUris = { "http://127.0.0.1", "sample-windows-client://callback" },
                    AllowOfflineAccess = true,
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "api-1", "api-2:read_only"
                    }
                },

                ///////////////////////////////////////////
                // Introspection Client Sample
                //////////////////////////////////////////
                new Client {
                    ClientId = "roclient.reference",
                    ClientSecrets = {
                        new Secret("secret".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes = { "api-1", "api-2:read_only" },
                    AccessTokenType = AccessTokenType.Reference
                },

                ///////////////////////////////////////////
                // MVC Implicit Flow Samples
                //////////////////////////////////////////
                new Client {
                    ClientId = "mvc.implicit",
                    ClientName = "MVC Implicit",
                    ClientUri = "http://identityserver.io",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris =  { "http://localhost:44077/signin-oidc" },
                    FrontChannelLogoutUri = "http://localhost:44077/signout-oidc",
                    PostLogoutRedirectUris = { "http://localhost:44077/signout-callback-oidc" },
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "api-1", "api-2:read_only"
                    }
                },

                ///////////////////////////////////////////
                // MVC Manual Implicit Flow Sample
                //////////////////////////////////////////
                new Client {
                    ClientId = "mvc.manual",
                    ClientName = "MVC Manual",
                    ClientUri = "http://identityserver.io",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RedirectUris = { "http://localhost:44078/home/callback" },
                    PostLogoutRedirectUris = { "http://localhost:44078/" },
                    FrontChannelLogoutUri = "http://localhost:44078/home/FrontChannelLogout",
                    BackChannelLogoutUri = "http://localhost:44078/home/BackChannelLogout",
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                    }
                },

                ///////////////////////////////////////////
                // MVC Hybrid Flow Sample
                //////////////////////////////////////////
                new Client {
                    ClientId = "mvc.hybrid",
                    ClientName = "MVC Hybrid",
                    ClientUri = "http://identityserver.io",
                    LogoUri = "https://pbs.twimg.com/profile_images/1612989113/Ki-hanja_400x400.png",
                    ClientSecrets = {
                        new Secret("secret".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    AllowAccessTokensViaBrowser = false,
                    RedirectUris = { "http://localhost:21402/signin-oidc" },
                    FrontChannelLogoutUri = "http://localhost:21402/signout-oidc",
                    PostLogoutRedirectUris = { "http://localhost:21402/signout-callback-oidc" },
                    AllowOfflineAccess = true,
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "api-1", "api-2:read_only"
                    }
                },
                ///////////////////////////////////////////
                // MVC Hybrid Flow Sample (Back Channel logout)
                //////////////////////////////////////////
                new Client {
                    ClientId = "mvc.hybrid.backchannel",
                    ClientName = "MVC Hybrid (with BackChannel logout)",
                    ClientUri = "http://identityserver.io",
                    ClientSecrets = {
                        new Secret("secret".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    AllowAccessTokensViaBrowser = false,
                    RedirectUris = { "http://localhost:21403/signin-oidc" },
                    BackChannelLogoutUri = "http://localhost:21403/logout",
                    PostLogoutRedirectUris = { "http://localhost:21403/signout-callback-oidc" },
                    AllowOfflineAccess = true,
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "api-1", "api-2:read_only"
                    }
                },

                ///////////////////////////////////////////
                // MVC Hybrid Flow Sample (Automatic Refresh)
                //////////////////////////////////////////
                new Client {
                    ClientId = "mvc.hybrid.autorefresh",
                    ClientName = "MVC Hybrid (with automatic refresh)",
                    ClientUri = "http://identityserver.io",
                    ClientSecrets = {
                        new Secret("secret".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    AllowAccessTokensViaBrowser = false,
                    AccessTokenLifetime = 75,
                    RedirectUris = { "http://localhost:21404/signin-oidc" },
                    FrontChannelLogoutUri = "http://localhost:21404/signout-oidc",
                    PostLogoutRedirectUris = { "http://localhost:21404/signout-callback-oidc" },
                    AllowOfflineAccess = true,
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "api-1", "api-2:read_only"
                    }
                },

                ///////////////////////////////////////////
                // JS OAuth 2.0 Sample
                //////////////////////////////////////////
                new Client {
                    ClientId = "js_oauth",
                    ClientName = "JavaScript OAuth 2.0 Client",
                    ClientUri = "http://identityserver.io",
                    LogoUri = "https://pbs.twimg.com/profile_images/1612989113/Ki-hanja_400x400.png",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = { "http://localhost:28895/index.html" },
                    AllowedScopes = { "api-1", "api-2:read_only" }
                },
                
                ///////////////////////////////////////////
                // JS OIDC Sample
                //////////////////////////////////////////
                new Client {
                    ClientId = "js_oidc",
                    ClientName = "JavaScript OIDC Client",
                    ClientUri = "http://identityserver.io",
                    LogoUri = "https://pbs.twimg.com/profile_images/1612989113/Ki-hanja_400x400.png",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RequireClientSecret = false,
                    AccessTokenType = AccessTokenType.Jwt,
                    RedirectUris = {
                        "http://localhost:7017/index.html",
                        "http://localhost:7017/callback.html",
                        "http://localhost:7017/silent.html",
                        "http://localhost:7017/popup.html"
                    },
                    PostLogoutRedirectUris = { "http://localhost:7017/index.html" },
                    AllowedCorsOrigins = { "http://localhost:7017" },
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "api-1", "api-2:read_only", "api-2:full_access"
                    }
                }
            };
        }


        protected IdentityDataContext<TId, TUser, TRole> Context { get; }

        protected UserManager<TUser> UserManager { get; }

        public IdentityDataContextSeeder(IdentityDataContext<TId, TUser, TRole> context, UserManager<TUser> userManager)
        {
            Context = context;

            UserManager = userManager;
        }

        public async Task SeedAsync()
        {
            foreach (var user in GetUsers())
            {
                await UserManager.CreateAsync(user, "@1w2E3r4");

                await UserManager.AddClaimsAsync(user, new[] {
                    new Claim(JwtClaimTypes.Name, "User Name"),
                    new Claim(JwtClaimTypes.GivenName, "User"),
                    new Claim(JwtClaimTypes.FamilyName, "Name"),
                    new Claim(JwtClaimTypes.Email, user.Email),
                    new Claim(JwtClaimTypes.EmailVerified, user.EmailConfirmed.ToString(), ClaimValueTypes.Boolean)
                });
            }

            foreach (var client in GetClients())
            {
                Context.Clients.Add(client.ToEntity());
            }

            Context.SaveChanges();

            foreach (var resource in GetIdentityResources())
            {
                Context.IdentityResources.Add(resource.ToEntity());
            }

            Context.SaveChanges();

            foreach (var resource in GetApis())
            {
                Context.ApiResources.Add(resource.ToEntity());
            }

            Context.SaveChanges();
        }
    }
}
