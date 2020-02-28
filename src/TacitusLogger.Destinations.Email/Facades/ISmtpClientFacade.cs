using MailKit.Net.Smtp;
using MimeKit;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TacitusLogger.Destinations.Email
{
    internal interface ISmtpClientFacade
    {
        void SendEmail(SmtpClient smtpClient, MailboxAddress from, ICollection<string> recipients, string subject, string body, string attachment = null);
        Task SendEmailAsync(SmtpClient smtpClient, MailboxAddress from, ICollection<string> recipients, string subject, string body, string attachment = null, CancellationToken cancellationToken = default(CancellationToken));
    }
}
