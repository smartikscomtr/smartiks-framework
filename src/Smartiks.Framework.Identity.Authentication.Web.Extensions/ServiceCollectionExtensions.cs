using System;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Smartiks.Framework.Identity.Authentication.Web.Abstractions;

namespace Smartiks.Framework.Identity.Authentication.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers the IdentityServer authentication handler.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the authentication handler to.</param>
        /// <param name="optionsAction">The configure options.</param>
        /// <returns></returns>
        public static void AddIdentityAuthentication(this IServiceCollection services, Action<IdentityAuthenticationClientOptions> optionsAction)
        {
            var options = new IdentityAuthenticationClientOptions();

            optionsAction?.Invoke(options);

            services
                .AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(optionsBuilder => {
                    optionsBuilder.Authority = options.Authority;
                    optionsBuilder.ApiName = options.ApiName;
                    optionsBuilder.ApiSecret = options.ApiSecret;
                    optionsBuilder.RequireHttpsMetadata = false;
                });
        }
    }
}
