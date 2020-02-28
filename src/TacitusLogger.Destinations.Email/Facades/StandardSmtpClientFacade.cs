using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TacitusLogger.Destinations.Email
{
    internal class StandardSmtpClientFacade : ISmtpClientFacade
    {
        public void SendEmail(SmtpClient smtpClient, MailboxAddress from, ICollection<string> recipients, string subject, string body, string attachment = null)
        {
            smtpClient.Send(PrepareMessage(from, recipients, subject, body, attachment));
        }
        public Task SendEmailAsync(SmtpClient smtpClient, MailboxAddress from, ICollection<string> recipients, string subject, string body, string attachment = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (cancellationToken.IsCancellationRequested)
                return Task.FromCanceled(cancellationToken);
            return smtpClient.SendAsync(PrepareMessage(from, recipients, subject, body, attachment), cancellationToken);
        }
        private MimeMessage PrepareMessage(MailboxAddress from, ICollection<string> recipients, string subject, string body, string attachment)
        {
            if (from == null)
                throw new ArgumentNullException("from");
            if (recipients == null)
                throw new ArgumentNullException("recipients");
            if (recipients.Count == 0)
                throw new ArgumentException("Recipients list is empty");
            if (recipients.Contains(null))
                throw new ArgumentException("Recipients list contains null values");
            if (subject == null)
                subject = "";
            if (body == null)
                body = "";

            var message = new MimeMessage();
            message.From.Add(from);
            foreach (var recipient in recipients)
                message.To.Add(new MailboxAddress(recipient));

            message.Subject = subject;
            var textBodyPart = new TextPart("plain")
            {
                Text = body
            };

            if (attachment != null)
            {
                var bytes = Encoding.UTF8.GetBytes(attachment);
                var attachmentPart = new MimePart("text", "plain")
                {
                    Content = new MimeContent(new MemoryStream(bytes)),
                    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                    FileName = "Log attachment.txt"
                };
                var multipart = new Multipart("mixed");
                multipart.Add(textBodyPart);
                multipart.Add(attachmentPart);
            }
            else
                message.Body = textBodyPart;

            return message;
        }
    }
}
