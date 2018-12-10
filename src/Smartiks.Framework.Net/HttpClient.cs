using System.Net.Http;
using Microsoft.Extensions.Options;
using Smartiks.Framework.Net.Abstractions;

namespace Smartiks.Framework.Net
{
    public class HttpClient<TOptions> : HttpClient
        where TOptions : HttpClientOptions, new()
    {
        public TOptions Options { get; }

        public HttpClient(IOptions<TOptions> optionsProvider)
        {
            Options = optionsProvider.Value;

            Timeout = Options.Timeout;
        }
    }
}
