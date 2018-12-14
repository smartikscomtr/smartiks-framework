using System;
using IdentityServer3.AccessTokenValidation;
using Owin;
using Smartiks.Framework.Identity.Authentication.Owin.Abstractions;

namespace Smartiks.Framework.Identity.Authentication.Owin.Extensions
{
    public static class AppBuilderExtensions
    {
        public static void UseIdentityAuthentication(this IAppBuilder app, Action<IdentityAuthenticationClientOptions> optionsAction)
        {
            var options = new IdentityAuthenticationClientOptions();

            optionsAction?.Invoke(options);

            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions {
                Authority = options.Authority,
                DelayLoadMetadata = true
            });
        }
    }
}
