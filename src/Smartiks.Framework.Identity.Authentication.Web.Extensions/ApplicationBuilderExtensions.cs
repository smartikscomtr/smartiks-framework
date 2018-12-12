using Microsoft.AspNetCore.Builder;

namespace Smartiks.Framework.Identity.Authentication.Web.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds the <see cref="T:Microsoft.AspNetCore.Authentication.AuthenticationMiddleware" /> to the specified <see cref="T:Microsoft.AspNetCore.Builder.IApplicationBuilder" />, which enables authentication capabilities.
        /// </summary>
        /// <param name="app">The <see cref="T:Microsoft.AspNetCore.Builder.IApplicationBuilder" /> to add the middleware to.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static void UseIdentityAuthentication(this IApplicationBuilder app)
        {
            app.UseAuthentication();
        }
    }
}
