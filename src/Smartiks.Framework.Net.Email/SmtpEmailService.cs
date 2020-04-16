using Microsoft.Extensions.Options;
using Smartiks.Framework.Net.Email.Abstractions;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace Smartiks.Framework.Net.Email
{
    /// <summary>
    /// Smtp Email Service
    /// </summary>
    public class SmtpEmailService : IEmailService, IDisposable
    {
        protected SmtpEmailOptions Options { get; set; }

        protected SmtpClient Client { get; set; }

        public SmtpEmailService(IOptions<SmtpEmailOptions> options)
        {
            Options = options.Value;

            Client =
                new SmtpClient
                {
                    Host = Options.Hostname,
                    Port = Options.Port,
                    EnableSsl = Options.UseSecureConnection,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(Options.Username, Options.Password),
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };
        }

        public async Task SendEmailAsync(EmailMessage message, CancellationToken token = default)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            using (var mailMessage = new MailMessage())
            {
                if (!string.IsNullOrEmpty(message.From))
                {
                    mailMessage.From = new MailAddress(message.From);
                }

                if (!string.IsNullOrEmpty(message.Sender))
                {
                    mailMessage.From = new MailAddress(message.Sender);
                }

                if (message.ReplyTo?.Count > 0)
                {
                    foreach (var address in message.ReplyTo)
                    {
                        mailMessage.ReplyToList.Add(new MailAddress(address));
                    }
                }

                if (message.To?.Count > 0)
                {
                    foreach (var address in message.To)
                    {
                        mailMessage.To.Add(new MailAddress(address));
                    }
                }

                if (message.Cc?.Count > 0)
                {
                    foreach (var address in message.Cc)
                    {
                        mailMessage.CC.Add(new MailAddress(address));
                    }
                }

                if (message.Bcc?.Count > 0)
                {
                    foreach (var address in message.Bcc)
                    {
                        mailMessage.Bcc.Add(new MailAddress(address));
                    }
                }

                if (message.Headers?.Count > 0)
                {
                    foreach (var header in message.Headers)
                    {
                        mailMessage.Headers.Add(header.Key, header.Value);
                    }
                }

                if (!string.IsNullOrEmpty(message.Subject))
                {
                    mailMessage.Subject = message.Subject;
                }

                if (!string.IsNullOrEmpty(message.Body))
                {
                    mailMessage.Body = message.Body;
                }

                mailMessage.IsBodyHtml = message.IsBodyHtml;
                
                await Client.SendMailAsync(mailMessage);
            }
        }

        public void Dispose()
        {
            Client?.Dispose();
        }
    }
}