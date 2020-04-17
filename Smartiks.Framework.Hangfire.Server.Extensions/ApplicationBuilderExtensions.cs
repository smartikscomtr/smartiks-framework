using Hangfire;
using Microsoft.AspNetCore.Builder;

namespace Smartiks.Framework.TaskScheduler.Server.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseTaskSchedulerDashboard(this IApplicationBuilder app)
        {
            app.UseHangfireDashboard();
        }
    }
}