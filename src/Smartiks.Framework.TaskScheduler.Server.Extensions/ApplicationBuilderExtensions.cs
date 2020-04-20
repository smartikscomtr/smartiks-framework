using Hangfire;
using Microsoft.AspNetCore.Builder;

namespace Smartiks.Framework.TaskScheduler.Server.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseTaskSchedulerDashboard(this IApplicationBuilder app, string path = "/hangfire", DashboardOptions options = null, JobStorage storage = null)
        {
            app.UseHangfireDashboard(path, options, storage);
        }
    }
}