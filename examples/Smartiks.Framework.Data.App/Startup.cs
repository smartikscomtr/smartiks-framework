using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Smartiks.Framework.Data.App.Data;
using Smartiks.Framework.Data.EntityFramework;
using Smartiks.Framework.Data.Web;

namespace Smartiks.Framework.Data.App
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
            services.AddDbContext<DataContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("DataContext"))
            );

            var mapperConfiguration =
               new MapperConfiguration(
                   configure => configure.AddProfile(new MapperProfile())
               );

            var mapper = mapperConfiguration.CreateMapper();

            services.AddSingleton(mapper);

            services.AddTransient(typeof(ContextRepository<,,,>));

            services
                .AddMvc(options =>
                {
                    options.ModelBinderProviders.Insert(0, new KendoQueryModelBinderProvider());
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
