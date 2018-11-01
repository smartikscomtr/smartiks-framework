using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Smartiks.Framework.Net.Email.Abstractions;

namespace Smartiks.Framework.Net.Email
{
    /// <summary>
    /// Smtp Email Service
    /// </summary>
    public class SmtpEmailService : IEmailService
    {
        protected SmtpEmailOptions Options { get; set; }

        public SmtpEmailService(IOptions<SmtpEmailOptions> options)
        {
            Options = options.Value;
        }

        public async Task SendEmailAsync(MailMessage message, CancellationToken token = default(CancellationToken))
        {
            var client =
                new SmtpClient {
                    Host = Options.Hostname,
                    Port = Options.Port,
                    EnableSsl = Options.UseSecureConnection,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(Options.Username, Options.Password),
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };

            await client.SendMailAsync(message);
        }
    }
}
