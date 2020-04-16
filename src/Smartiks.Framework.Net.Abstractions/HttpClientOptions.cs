using System;
using System.Collections.Generic;

namespace Smartiks.Framework.Net.Abstractions
{
    public class HttpClientOptions
    {
        public Uri BaseAddress { get; set; }

        public IDictionary<string, IReadOnlyCollection<string>> DefaultRequestHeaders { get; set; }

        public TimeSpan? Timeout { get; set; }
    }
}