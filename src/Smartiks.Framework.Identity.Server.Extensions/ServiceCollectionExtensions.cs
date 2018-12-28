using IdentityServer4.EntityFramework.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Smartiks.Framework.Identity.Server.Abstractions;
using System;

namespace Smartiks.Framework.Identity.Server.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIdentityAuthenticationServer<TContext, TUser, TRole>(this IServiceCollection services, Action<IdentityAuthenticationServerOptions> optionsAction)
            where TContext : DbContext, IConfigurationDbContext, IPersistedGrantDbContext
            where TUser : class
            where TRole : class
        {
            services
                .AddIdentity<TUser, TRole>()
                .AddEntityFrameworkStores<TContext>();

            return
                services
                    .AddIdentityAuthenticationServerCore<TContext, TUser, TRole>(optionsAction);
        }

        public static IServiceCollection AddIdentityAuthenticationServerCore<TContext, TUser, TRole>(this IServiceCollection services, Action<IdentityAuthenticationServerOptions> optionsAction)
            where TContext : DbContext, IConfigurationDbContext, IPersistedGrantDbContext
            where TUser : class
            where TRole : class
        {
            var options = new IdentityAuthenticationServerOptions();

            optionsAction?.Invoke(options);

            var identityServerBuilder =
                services
                    .AddIdentityServer(optionsBuilder =>
                    {
                        optionsBuilder.Events.RaiseInformationEvents = true;
                        optionsBuilder.Events.RaiseErrorEvents = true;
                        optionsBuilder.Events.RaiseFailureEvents = true;
                        optionsBuilder.Events.RaiseSuccessEvents = true;
                    })
                    .AddAspNetIdentity<TUser>()
                    .AddConfigurationStore<TContext>(optionsBuilder =>
                    {
                        optionsBuilder.ConfigureDbContext = options.DbContextOptionsBuilder;
                    })
                    .AddOperationalStore<TContext>(optionsBuilder =>
                    {
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

            return services;
        }
    }
}