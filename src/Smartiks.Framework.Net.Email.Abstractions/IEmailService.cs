using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace Smartiks.Framework.Net.Email.Abstractions
{
    public interface IEmailService
    {
        Task SendEmailAsync(MailMessage message, CancellationToken token = default(CancellationToken));
    }
}
