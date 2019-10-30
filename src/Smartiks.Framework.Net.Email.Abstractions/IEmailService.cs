using System.Threading;
using System.Threading.Tasks;

namespace Smartiks.Framework.Net.Email.Abstractions
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailMessage message, CancellationToken token = default);
    }
}