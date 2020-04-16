using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Smartiks.Framework.Identity.Data;
using Smartiks.Framework.Identity.Data.Abstractions;
using Smartiks.Framework.Identity.Server.Extensions;
using System;
using System.Security.Cryptography;

namespace Smartiks.Framework.Identity.Server.App
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<IdentityDataContext<int, User<int>, Role<int>>>(optionsBuilder => {
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("IdentityDataContext"));
            });

            services.AddIdentityAuthenticationServer<IdentityDataContext<int, User<int>, Role<int>>, User<int>, Role<int>>(options => {
                var rsaCryptoServiceProvider = new RSACryptoServiceProvider(2048);

                var rsaParameters = rsaCryptoServiceProvider.ExportParameters(true);

                var rsaSecurityKey = new RsaSecurityKey(rsaParameters) { KeyId = Guid.NewGuid().ToString("N") };

                options.SigningCredentials = new SigningCredentials(rsaSecurityKey, "RS256");

                options.DbContextOptionsBuilder = x => x.UseSqlServer(Configuration.GetConnectionString("IdentityDataContext"));
            });

            services.Configure<CookiePolicyOptions>(options => {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddScoped<Seeder>();

            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;

            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IApplicationLifetime applicationLifetime, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                applicationLifetime.ApplicationStarted.Register(async () => {

                    using (var serviceScope = app.ApplicationServices.CreateScope())
                    {
                        var identityDataContext = serviceScope.ServiceProvider.GetRequiredService<IdentityDataContext<int, User<int>, Role<int>>>();

                        if (identityDataContext.Database.EnsureCreated())
                        {
                            var seeder = serviceScope.ServiceProvider.GetRequiredService<Seeder>();

                            await seeder.SeedAsync();
                        }
                    }
                });

                app.UseDeveloperExceptionPage();

                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");

                //app.UseHsts();
            }

            //app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseCookiePolicy();

            app.UseIdentityAuthenticationServer();

            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
