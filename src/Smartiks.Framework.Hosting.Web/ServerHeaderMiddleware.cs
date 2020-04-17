using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Smartiks.Framework.Hosting.Web.Abstractions;

namespace Smartiks.Framework.Hosting.Web
{
    public class ServerHeaderMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ServerHeaderOptions _options;

        public ServerHeaderMiddleware(RequestDelegate next, IOptions<ServerHeaderOptions> optionsProvider)
        {
            this._next = next;

            _options = optionsProvider.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var response = context.Response;

            response.OnStarting(() => {

                response.Headers.Add("Server", _options.Name);

                return Task.CompletedTask;
            });

            await _next.Invoke(context);
        }
    }
}
