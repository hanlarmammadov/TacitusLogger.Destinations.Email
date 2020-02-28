using MailKit.Net.Smtp;
using MimeKit;
using TacitusLogger.Builders;
using TacitusLogger.Serializers;

namespace TacitusLogger.Destinations.Email
{
    public interface ISmtpEmailDestinationBuilder : IDestinationBuilder
    {
        ISmtpEmailDestinationBuilder WithSmtpClient(SmtpClient smtpClient, MailboxAddress fromAddress);
        ISmtpEmailDestinationBuilder WithSmtpClient(SmtpClient smtpClient, string fromAddress);
        ISmtpEmailDestinationBuilder WithRecipients(IRecipientProvider recipientProvider);
        ISmtpEmailDestinationBuilder WithSubject(ILogSerializer mailSubjectGenerator);
        ISmtpEmailDestinationBuilder WithBody(ILogSerializer mailBodyGenerator);
        ISmtpEmailDestinationBuilder WithAttachment(ILogSerializer mailAttachmentContentGenerator);
    }
}
