using Microsoft.AspNetCore.Builder;

namespace Smartiks.Framework.Identity.Server.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseIdentityAuthenticationServer(this IApplicationBuilder app)
        {
            app.UseIdentityServer();
        }
    }
}
