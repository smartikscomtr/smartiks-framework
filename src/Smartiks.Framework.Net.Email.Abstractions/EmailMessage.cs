using System.Collections.Generic;

namespace Smartiks.Framework.Net.Email.Abstractions
{
    public class EmailMessage
    {
        public string From { get; set; }

        public string Sender { get; set; }

        public IReadOnlyCollection<string> ReplyTo { get; set; }

        public IReadOnlyCollection<string> To { get; set; }

        public IReadOnlyCollection<string> Cc { get; set; }

        public IReadOnlyCollection<string> Bcc { get; set; }

        public IDictionary<string, string> Headers { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public bool IsBodyHtml { get; set; } = true;
    }
}