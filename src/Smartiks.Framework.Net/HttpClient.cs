using Microsoft.Extensions.Options;
using Smartiks.Framework.Net.Abstractions;
using System.Net.Http;

namespace Smartiks.Framework.Net
{
    public class HttpClient<TOptions> : HttpClient
        where TOptions : HttpClientOptions, new()
    {
        public TOptions Options { get; }

        public HttpClient(IOptions<TOptions> optionsProvider)
        {
            Options = optionsProvider.Value;

            if (Options.BaseAddress != null)
            {
                BaseAddress = Options.BaseAddress;
            }

            if (Options.DefaultRequestHeaders?.Count > 0)
            {
                DefaultRequestHeaders.Clear();

                foreach (var header in Options.DefaultRequestHeaders)
                {
                    DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }

            if (Options.Timeout.HasValue)
            {
                Timeout = Options.Timeout.Value;
            }
        }
    }
}