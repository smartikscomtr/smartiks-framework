using System;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Smartiks.Framework.Identity.Data.Abstractions;

namespace Smartiks.Framework.Identity.Data
{
    public class IdentityDataContext<TId, TUser, TRole> : IdentityDbContext<TUser, TRole, TId>, IConfigurationDbContext, IPersistedGrantDbContext
        where TId : IEquatable<TId>
        where TUser : User<TId>
        where TRole : Role<TId>
    {
        public IdentityDataContext(DbContextOptions<IdentityDataContext<TId, TUser, TRole>> options) : base(options)
        {
        }

        #region DbSets

        public DbSet<Client> Clients { get; set; }

        public DbSet<ApiResource> ApiResources { get; set; }

        public DbSet<IdentityResource> IdentityResources { get; set; }

        public DbSet<PersistedGrant> PersistedGrants { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region User
            modelBuilder.Entity<TUser>(b => { b.ToTable("Users"); }); 
            #endregion

            #region Role
            modelBuilder.Entity<TRole>(b => { b.ToTable("Roles"); }); 
            #endregion

            #region UserRole
            modelBuilder.Entity<IdentityUserRole<TId>>(b => { b.ToTable("UserRoles"); }); 
            #endregion

            #region UserClaim
            modelBuilder.Entity<IdentityUserClaim<TId>>(b => { b.ToTable("UserClaims"); }); 
            #endregion

            #region UserLogin
            modelBuilder.Entity<IdentityUserLogin<TId>>(b => { b.ToTable("UserLogins"); }); 
            #endregion

            #region UserToken
            modelBuilder.Entity<IdentityUserToken<TId>>(b => { b.ToTable("UserTokens"); }); 
            #endregion

            #region RoleClaim
            modelBuilder.Entity<IdentityRoleClaim<TId>>(b => { b.ToTable("RoleClaims"); }); 
            #endregion

            #region ApiResource
            modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ApiResource", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<string>("Description")
                    .HasMaxLength(1000);

                b.Property<string>("DisplayName")
                    .HasMaxLength(200);

                b.Property<bool>("Enabled");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(200);

                b.HasKey("Id");

                b.HasIndex("Name")
                    .IsUnique();

                b.ToTable("ApiResources");
            });
            #endregion

            #region ApiResourceClaim
            modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ApiResourceClaim", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<int?>("ApiResourceId")
                    .IsRequired();

                b.Property<string>("Type")
                    .IsRequired()
                    .HasMaxLength(200);

                b.HasKey("Id");

                b.HasIndex("ApiResourceId");

                b.ToTable("ApiClaims");
            });
            #endregion

            #region ApiScope
            modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ApiScope", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<int?>("ApiResourceId")
                    .IsRequired();

                b.Property<string>("Description")
                    .HasMaxLength(1000);

                b.Property<string>("DisplayName")
                    .HasMaxLength(200);

                b.Property<bool>("Emphasize");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(200);

                b.Property<bool>("Required");

                b.Property<bool>("ShowInDiscoveryDocument");

                b.HasKey("Id");

                b.HasIndex("ApiResourceId");

                b.HasIndex("Name")
                    .IsUnique();

                b.ToTable("ApiScopes");
            });
            #endregion

            #region ApiScopeClaim
            modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ApiScopeClaim", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<int?>("ApiScopeId")
                    .IsRequired();

                b.Property<string>("Type")
                    .IsRequired()
                    .HasMaxLength(200);

                b.HasKey("Id");

                b.HasIndex("ApiScopeId");

                b.ToTable("ApiScopeClaims");
            });
            #endregion

            #region ApiSecret
            modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ApiSecret", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<int?>("ApiResourceId")
                    .IsRequired();

                b.Property<string>("Description")
                    .HasMaxLength(1000);

                b.Property<DateTime?>("Expiration");

                b.Property<string>("Type")
                    .HasMaxLength(250);

                b.Property<string>("Value")
                    .HasMaxLength(2000);

                b.HasKey("Id");

                b.HasIndex("ApiResourceId");

                b.ToTable("ApiSecrets");
            });
            #endregion

            #region Client
            modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.Client", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<int>("AbsoluteRefreshTokenLifetime");

                b.Property<int>("AccessTokenLifetime");

                b.Property<int>("AccessTokenType");

                b.Property<bool>("AllowAccessTokensViaBrowser");

                b.Property<bool>("AllowOfflineAccess");

                b.Property<bool>("AllowPlainTextPkce");

                b.Property<bool>("AllowRememberConsent");

                b.Property<bool>("AlwaysIncludeUserClaimsInIdToken");

                b.Property<bool>("AlwaysSendClientClaims");

                b.Property<int>("AuthorizationCodeLifetime");

                b.Property<bool>("BackChannelLogoutSessionRequired");

                b.Property<string>("BackChannelLogoutUri")
                    .HasMaxLength(2000);

                b.Property<string>("ClientClaimsPrefix")
                    .HasMaxLength(200);

                b.Property<string>("ClientId")
                    .IsRequired()
                    .HasMaxLength(200);

                b.Property<string>("ClientName")
                    .HasMaxLength(200);

                b.Property<string>("ClientUri")
                    .HasMaxLength(2000);

                b.Property<int?>("ConsentLifetime");

                b.Property<string>("Description")
                    .HasMaxLength(1000);

                b.Property<bool>("EnableLocalLogin");

                b.Property<bool>("Enabled");

                b.Property<bool>("FrontChannelLogoutSessionRequired");

                b.Property<string>("FrontChannelLogoutUri")
                    .HasMaxLength(2000);

                b.Property<int>("IdentityTokenLifetime");

                b.Property<bool>("IncludeJwtId");

                b.Property<string>("LogoUri")
                    .HasMaxLength(2000);

                b.Property<string>("PairWiseSubjectSalt")
                    .HasMaxLength(200);

                b.Property<string>("ProtocolType")
                    .IsRequired()
                    .HasMaxLength(200);

                b.Property<int>("RefreshTokenExpiration");

                b.Property<int>("RefreshTokenUsage");

                b.Property<bool>("RequireClientSecret");

                b.Property<bool>("RequireConsent");

                b.Property<bool>("RequirePkce");

                b.Property<int>("SlidingRefreshTokenLifetime");

                b.Property<bool>("UpdateAccessTokenClaimsOnRefresh");

                b.HasKey("Id");

                b.HasIndex("ClientId")
                    .IsUnique();

                b.ToTable("Clients");
            });
            #endregion

            #region ClientClaim
            modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientClaim", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<int?>("ClientId")
                    .IsRequired();

                b.Property<string>("Type")
                    .IsRequired()
                    .HasMaxLength(250);

                b.Property<string>("Value")
                    .IsRequired()
                    .HasMaxLength(250);

                b.HasKey("Id");

                b.HasIndex("ClientId");

                b.ToTable("ClientClaims");
            });
            #endregion

            #region ClientCorsOrigin
            modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientCorsOrigin", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<int?>("ClientId")
                    .IsRequired();

                b.Property<string>("Origin")
                    .IsRequired()
                    .HasMaxLength(150);

                b.HasKey("Id");

                b.HasIndex("ClientId");

                b.ToTable("ClientCorsOrigins");
            });
            #endregion

            #region ClientGrantType
            modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientGrantType", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<int?>("ClientId")
                    .IsRequired();

                b.Property<string>("GrantType")
                    .IsRequired()
                    .HasMaxLength(250);

                b.HasKey("Id");

                b.HasIndex("ClientId");

                b.ToTable("ClientGrantTypes");
            });
            #endregion

            #region ClientIdPRestriction
            modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientIdPRestriction", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<int?>("ClientId")
                    .IsRequired();

                b.Property<string>("Provider")
                    .IsRequired()
                    .HasMaxLength(200);

                b.HasKey("Id");

                b.HasIndex("ClientId");

                b.ToTable("ClientIdPRestrictions");
            });
            #endregion

            #region ClientPostLogoutRedirectUri
            modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientPostLogoutRedirectUri", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<int?>("ClientId")
                    .IsRequired();

                b.Property<string>("PostLogoutRedirectUri")
                    .IsRequired()
                    .HasMaxLength(2000);

                b.HasKey("Id");

                b.HasIndex("ClientId");

                b.ToTable("ClientPostLogoutRedirectUris");
            });
            #endregion

            #region ClientProperty
            modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientProperty", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<int?>("ClientId")
                    .IsRequired();

                b.Property<string>("Key")
                    .IsRequired()
                    .HasMaxLength(250);

                b.Property<string>("Value")
                    .IsRequired()
                    .HasMaxLength(2000);

                b.HasKey("Id");

                b.HasIndex("ClientId");

                b.ToTable("ClientProperties");
            });
            #endregion

            #region ClientRedirectUri
            modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientRedirectUri", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<int?>("ClientId")
                    .IsRequired();

                b.Property<string>("RedirectUri")
                    .IsRequired()
                    .HasMaxLength(2000);

                b.HasKey("Id");

                b.HasIndex("ClientId");

                b.ToTable("ClientRedirectUris");
            });
            #endregion

            #region ClientScope
            modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientScope", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<int?>("ClientId")
                    .IsRequired();

                b.Property<string>("Scope")
                    .IsRequired()
                    .HasMaxLength(200);

                b.HasKey("Id");

                b.HasIndex("ClientId");

                b.ToTable("ClientScopes");
            });
            #endregion

            #region ClientSecret
            modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientSecret", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<int?>("ClientId")
                    .IsRequired();

                b.Property<string>("Description")
                    .HasMaxLength(2000);

                b.Property<DateTime?>("Expiration");

                b.Property<string>("Type")
                    .HasMaxLength(250);

                b.Property<string>("Value")
                    .IsRequired()
                    .HasMaxLength(2000);

                b.HasKey("Id");

                b.HasIndex("ClientId");

                b.ToTable("ClientSecrets");
            });
            #endregion

            #region IdentityClaim
            modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.IdentityClaim", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<int?>("IdentityResourceId")
                    .IsRequired();

                b.Property<string>("Type")
                    .IsRequired()
                    .HasMaxLength(200);

                b.HasKey("Id");

                b.HasIndex("IdentityResourceId");

                b.ToTable("IdentityClaims");
            });
            #endregion

            #region IdentityResource
            modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.IdentityResource", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<string>("Description")
                    .HasMaxLength(1000);

                b.Property<string>("DisplayName")
                    .HasMaxLength(200);

                b.Property<bool>("Emphasize");

                b.Property<bool>("Enabled");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(200);

                b.Property<bool>("Required");

                b.Property<bool>("ShowInDiscoveryDocument");

                b.HasKey("Id");

                b.HasIndex("Name")
                    .IsUnique();

                b.ToTable("IdentityResources");
            });
            #endregion

            #region ApiResourceClaim
            modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ApiResourceClaim", b =>
            {
                b.HasOne("IdentityServer4.EntityFramework.Entities.ApiResource", "ApiResource")
                    .WithMany("UserClaims")
                    .HasForeignKey("ApiResourceId")
                    .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion

            #region ApiScope
            modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ApiScope", b =>
            {
                b.HasOne("IdentityServer4.EntityFramework.Entities.ApiResource", "ApiResource")
                    .WithMany("Scopes")
                    .HasForeignKey("ApiResourceId")
                    .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion

            #region ApiScopeClaim
            modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ApiScopeClaim", b =>
            {
                b.HasOne("IdentityServer4.EntityFramework.Entities.ApiScope", "ApiScope")
                    .WithMany("UserClaims")
                    .HasForeignKey("ApiScopeId")
                    .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion

            #region ApiSecret
            modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ApiSecret", b =>
            {
                b.HasOne("IdentityServer4.EntityFramework.Entities.ApiResource", "ApiResource")
                    .WithMany("Secrets")
                    .HasForeignKey("ApiResourceId")
                    .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion

            #region ClientClaim
            modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientClaim", b =>
            {
                b.HasOne("IdentityServer4.EntityFramework.Entities.Client", "Client")
                    .WithMany("Claims")
                    .HasForeignKey("ClientId")
                    .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion

            #region ClientCorsOrigin
            modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientCorsOrigin", b =>
            {
                b.HasOne("IdentityServer4.EntityFramework.Entities.Client", "Client")
                    .WithMany("AllowedCorsOrigins")
                    .HasForeignKey("ClientId")
                    .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion

            #region ClientGrantType
            modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientGrantType", b =>
            {
                b.HasOne("IdentityServer4.EntityFramework.Entities.Client", "Client")
                    .WithMany("AllowedGrantTypes")
                    .HasForeignKey("ClientId")
                    .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion

            #region ClientIdPRestriction
            modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientIdPRestriction", b =>
            {
                b.HasOne("IdentityServer4.EntityFramework.Entities.Client", "Client")
                    .WithMany("IdentityProviderRestrictions")
                    .HasForeignKey("ClientId")
                    .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion

            #region ClientPostLogoutRedirectUri
            modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientPostLogoutRedirectUri", b =>
            {
                b.HasOne("IdentityServer4.EntityFramework.Entities.Client", "Client")
                    .WithMany("PostLogoutRedirectUris")
                    .HasForeignKey("ClientId")
                    .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion

            #region ClientProperty
            modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientProperty", b =>
            {
                b.HasOne("IdentityServer4.EntityFramework.Entities.Client", "Client")
                    .WithMany("Properties")
                    .HasForeignKey("ClientId")
                    .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion

            #region ClientRedirectUri
            modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientRedirectUri", b =>
            {
                b.HasOne("IdentityServer4.EntityFramework.Entities.Client", "Client")
                    .WithMany("RedirectUris")
                    .HasForeignKey("ClientId")
                    .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion

            #region ClientScope
            modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientScope", b =>
            {
                b.HasOne("IdentityServer4.EntityFramework.Entities.Client", "Client")
                    .WithMany("AllowedScopes")
                    .HasForeignKey("ClientId")
                    .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion

            #region ClientSecret
            modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientSecret", b =>
            {
                b.HasOne("IdentityServer4.EntityFramework.Entities.Client", "Client")
                    .WithMany("ClientSecrets")
                    .HasForeignKey("ClientId")
                    .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion

            #region IdentityClaim
            modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.IdentityClaim", b =>
            {
                b.HasOne("IdentityServer4.EntityFramework.Entities.IdentityResource", "IdentityResource")
                    .WithMany("UserClaims")
                    .HasForeignKey("IdentityResourceId")
                    .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion

            #region PersistedGrant
            modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.PersistedGrant", b =>
            {
                b.Property<string>("Key")
                    .HasMaxLength(200);

                b.Property<string>("ClientId")
                    .IsRequired()
                    .HasMaxLength(200);

                b.Property<DateTime>("CreationTime");

                b.Property<string>("Data")
                    .IsRequired()
                    .HasMaxLength(50000);

                b.Property<DateTime?>("Expiration");

                b.Property<string>("SubjectId")
                    .HasMaxLength(200);

                b.Property<string>("Type")
                    .IsRequired()
                    .HasMaxLength(50);

                b.HasKey("Key");

                b.HasIndex("SubjectId", "ClientId", "Type");

                b.ToTable("PersistedGrants");
            });
            #endregion
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}