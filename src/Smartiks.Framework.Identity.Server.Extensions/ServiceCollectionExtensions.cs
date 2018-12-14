using System;
using IdentityServer4.EntityFramework.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Smartiks.Framework.Identity.Server.Abstractions;

namespace Smartiks.Framework.Identity.Server.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddIdentityAuthenticationServer<TContext, TUser, TRole>(this IServiceCollection services, Action<IdentityAuthenticationServerOptions> optionsAction)
            where TContext : DbContext, IConfigurationDbContext, IPersistedGrantDbContext
            where TUser : class
            where TRole : class
        {
            services
                .AddIdentity<TUser, TRole>()
                .AddEntityFrameworkStores<TContext>();
                //.AddDefaultTokenProviders();

            //services
            //    .AddAuthentication();

            AddIdentityAuthenticationServerCore<TContext, TUser, TRole>(services, optionsAction);
        }

        public static void AddIdentityAuthenticationServerCore<TContext, TUser, TRole>(this IServiceCollection services, Action<IdentityAuthenticationServerOptions> optionsAction)
            where TContext : DbContext, IConfigurationDbContext, IPersistedGrantDbContext
            where TUser : class
            where TRole : class
        {
            var options = new IdentityAuthenticationServerOptions();

            optionsAction?.Invoke(options);

            var identityServerBuilder =
                services
                    .AddIdentityServer(optionsBuilder => {
                        optionsBuilder.Events.RaiseInformationEvents = true;
                        optionsBuilder.Events.RaiseErrorEvents = true;
                        optionsBuilder.Events.RaiseFailureEvents = true;
                        optionsBuilder.Events.RaiseSuccessEvents = true;
                    })
                    .AddAspNetIdentity<TUser>()
                    .AddConfigurationStore(optionsBuilder => {
                        optionsBuilder.ConfigureDbContext = options.DbContextOptionsBuilder;
                    })
                    .AddOperationalStore(optionsBuilder => {
                        optionsBuilder.ConfigureDbContext = options.DbContextOptionsBuilder;
                        optionsBuilder.EnableTokenCleanup = true;
                    });

            if (options.SigningCredentials != null)
            {
                identityServerBuilder.AddSigningCredential(options.SigningCredentials);
            }
            else
            {
                identityServerBuilder.AddDeveloperSigningCredential();
            }
        }
    }
}
