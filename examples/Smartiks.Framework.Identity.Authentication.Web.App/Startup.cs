using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Smartiks.Framework.Identity.Authentication.Web.Extensions;

namespace Smartiks.Framework.Identity.Authentication.Web.App
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentityAuthentication(optionsBuilder => {
                optionsBuilder.Authority = "http://localhost:5000";
                optionsBuilder.ApiName = "api";
                optionsBuilder.ApiSecret = "9afa571b-070a-4f08-8f73-0f6e0f51559e";
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseDatabaseErrorPage();
            }

            app.UseIdentityAuthentication();

            app.UseMvc();
        }
    }
}
