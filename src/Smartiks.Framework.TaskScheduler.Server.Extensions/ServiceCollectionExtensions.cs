using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.Extensions.DependencyInjection;

namespace Smartiks.Framework.TaskScheduler.Server.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTaskSchedulerService(this IServiceCollection services, string connectionString)
        {
            services.AddHangfire(config =>
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseDefaultTypeSerializer()
                    //.UseSqlServerStorage(connectionString)
                    .UseMemoryStorage());

            return services.AddHangfireServer();
        }
    }
}