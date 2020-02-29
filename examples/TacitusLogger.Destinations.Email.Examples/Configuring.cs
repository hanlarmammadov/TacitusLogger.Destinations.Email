using MailKit.Net.Smtp;
using MimeKit;
using Moq;
using Newtonsoft.Json;
using TacitusLogger.Serializers;

namespace TacitusLogger.Destinations.Email.Examples
{
    class Configuring
    {
        SmtpClient mailKitSmtpClient;
        string customBodyTemplate;
        string customAttachmentTemplate;

        public void Adding_Email_Destination_With_Minimal_Configuration()
        {
            MailboxAddress fromAddress = new MailboxAddress("sender", "sender@example.com");
            EmailDestination emailDestination = new EmailDestination(mailKitSmtpClient, fromAddress, "recipient@example.com");

            Logger logger = new Logger();
            logger.AddLogDestinations(emailDestination);

        }
        public void Adding_Email_Destination_With_Several_Recipients()
        {
            var recipients = new string[]
            {
                "recipient@example.com",
                "recipient@example.com",
                "recipient@example.com"
            };
            MailboxAddress fromAddress = new MailboxAddress("sender", "sender@example.com");
            EmailDestination emailDestination = new EmailDestination(mailKitSmtpClient, fromAddress, recipients);

            Logger logger = new Logger();
            logger.AddLogDestinations(emailDestination);
        }
        public void Adding_Email_Destination_With_Recipients_Function()
        {
            FactoryMethodRecipientProvider factoryMethodRecipientProvider = new FactoryMethodRecipientProvider((logModel) =>
            {
                if (logModel.LogTypeIsIn(LogType.Error, LogType.Failure, LogType.Critical))
                    return new string[] { "recipient1@example.com" };
                else
                    return new string[] { "recipient2@example.com" };
            });
            MailboxAddress fromAddress = new MailboxAddress("sender", "sender@example.com");
            EmailDestination emailDestination = new EmailDestination(mailKitSmtpClient, fromAddress, factoryMethodRecipientProvider);

            Logger logger = new Logger();
            logger.AddLogDestinations(emailDestination);
        }
        public void Adding_Email_Destination_With_Custom_Recipient_Provider()
        {
            IRecipientProvider customRecipientProvider = new Mock<IRecipientProvider>().Object;
            MailboxAddress fromAddress = new MailboxAddress("sender", "sender@example.com");
            EmailDestination emailDestination = new EmailDestination(mailKitSmtpClient, fromAddress, customRecipientProvider);

            Logger logger = new Logger();
            logger.AddLogDestinations(emailDestination);
        }
        public void Adding_Email_Destination_With_Custom_Subject_Body_And_Attachment_Templates()
        {
            EmailListRecipientProvider emailListRecipientProvider = new EmailListRecipientProvider("recipient@example.com");
            SimpleTemplateLogSerializer subjectTextSerializer = new SimpleTemplateLogSerializer("Notification from logger $Source: $LogType - $Description");
            ExtendedTemplateLogSerializer bodyLogSerializer = new ExtendedTemplateLogSerializer(customBodyTemplate);
            ExtendedTemplateLogSerializer attachmentLogSerializer = new ExtendedTemplateLogSerializer(customAttachmentTemplate);
            MailboxAddress fromAddress = new MailboxAddress("sender", "sender@example.com");

            EmailDestination emailDestination = new EmailDestination(mailKitSmtpClient,
                                                                     fromAddress,
                                                                     emailListRecipientProvider,
                                                                     subjectTextSerializer,
                                                                     bodyLogSerializer,
                                                                     attachmentLogSerializer);
            Logger logger = new Logger();
            logger.AddLogDestinations(emailDestination);
        }
        public void Adding_Email_Destination_With_Custom_Body_And_Attachment_Templates_And_JsonSettings()
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            EmailListRecipientProvider emailListRecipientProvider = new EmailListRecipientProvider("recipient@example.com");
            SimpleTemplateLogSerializer subjectTextSerializer = new SimpleTemplateLogSerializer("Notification from logger $Source: $LogType - $Description");
            ExtendedTemplateLogSerializer bodyLogSerializer = new ExtendedTemplateLogSerializer(customBodyTemplate, jsonSerializerSettings);
            ExtendedTemplateLogSerializer attachmentLogSerializer = new ExtendedTemplateLogSerializer(customAttachmentTemplate, jsonSerializerSettings);
            MailboxAddress fromAddress = new MailboxAddress("sender", "sender@example.com");

            EmailDestination emailDestination = new EmailDestination(mailKitSmtpClient,
                                                                     fromAddress,
                                                                     emailListRecipientProvider,
                                                                     subjectTextSerializer,
                                                                     bodyLogSerializer,
                                                                     attachmentLogSerializer);
            Logger logger = new Logger();
            logger.AddLogDestinations(emailDestination);
        }
        public void Adding_Email_Destination_With_Custom_Subject_Body_And_Attachment_Serializers()
        {
            ILogSerializer customSubjectSerializer = new Mock<ILogSerializer>().Object;
            ILogSerializer customBodySerializer = new Mock<ILogSerializer>().Object;
            ILogSerializer customAttachmentSerializer = new Mock<ILogSerializer>().Object;
            MailboxAddress fromAddress = new MailboxAddress("sender", "sender@example.com");
            EmailListRecipientProvider emailListRecipientProvider = new EmailListRecipientProvider("recipient@example.com");

            EmailDestination emailDestination = new EmailDestination(mailKitSmtpClient,
                                                                     fromAddress,
                                                                     emailListRecipientProvider,
                                                                     customSubjectSerializer,
                                                                     customBodySerializer,
                                                                     customAttachmentSerializer);
            Logger logger = new Logger();
            logger.AddLogDestinations(emailDestination);
        }
        public void Adding_Email_Destination_With_Custom_Subject_Text_Function()
        { 
            GeneratorFunctionLogSerializer subjectTextFunction = new GeneratorFunctionLogSerializer(m =>
            {
                return $"Notification from logger {m.Source}: {m.LogType} - {m.Description}";
            }); 
            MailboxAddress fromAddress = new MailboxAddress("sender", "sender@example.com");
            EmailListRecipientProvider emailListRecipientProvider = new EmailListRecipientProvider("recipient@example.com");

            EmailDestination emailDestination = new EmailDestination(mailKitSmtpClient,
                                                                     fromAddress,
                                                                     emailListRecipientProvider,
                                                                     subjectTextFunction);
            Logger logger = new Logger();
            logger.AddLogDestinations(emailDestination);
        }
    }
}
