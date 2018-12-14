using Microsoft.Owin;
using Owin;
using Smartiks.Framework.Identity.Authentication.Owin.Extensions;

[assembly: OwinStartup(typeof(Smartiks.Framework.Identity.Authentication.Owin.App.Startup))]

namespace Smartiks.Framework.Identity.Authentication.Owin.App
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseIdentityAuthentication(optionsConfig => {

                optionsConfig.Authority = "http://localhost:5000";
            });
        }
    }
}
