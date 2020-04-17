using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Smartiks.Framework.TaskScheduler.Server.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTaskSchedulerService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(config =>
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseDefaultTypeSerializer()
                    //.UseSqlServerStorage(configuration.GetConnectionString("Hangfire")) // TODO
                    .UseMemoryStorage());

            return services.AddHangfireServer();
        }
    }
}