using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Smartiks.Framework.Hosting.Web.Abstractions;

namespace Smartiks.Framework.Hosting.Web
{
    public class ServerHeaderMiddleware
    {
        private readonly RequestDelegate next;

        private readonly ServerHeaderOptions options;

        public ServerHeaderMiddleware(RequestDelegate next, IOptions<ServerHeaderOptions> optionsProvider)
        {
            this.next = next;

            options = optionsProvider.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var response = context.Response;

            response.OnStarting(() => {

                response.Headers.Add("Server", options.Name);

                return Task.CompletedTask;
            });

            await next.Invoke(context);
        }
    }
}
