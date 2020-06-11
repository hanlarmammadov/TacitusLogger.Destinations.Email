using MailKit.Net.Smtp;
using MimeKit;
using Moq;
using Newtonsoft.Json; 
using System.Collections.Generic;
using TacitusLogger.Builders;
using TacitusLogger.Serializers;

namespace TacitusLogger.Destinations.Email.Examples
{
    class ConfiguringWithBuilders
    {
        SmtpClient mailKitSmtpClient;
        string customBodyTemplate;
        string customAttachmentTemplate;

        public void Adding_Email_Destination_With_Minimal_Configuration()
        {
            var logger = LoggerBuilder.Logger()
                                      .ForAllLogs()
                                      .Email()
                                          .WithRecipients("recipient@example.com")
                                          .WithSmtpClient(mailKitSmtpClient, "sender@example.com")
                                          .Add()
                                      .BuildLogger();

        }
        public void Adding_Email_Destination_With_Several_Recipients()
        {
            var recipients = new string[] 
            {
                "recipient@example.com",
                "recipient@example.com",
                "recipient@example.com"
            };
            var logger = LoggerBuilder.Logger()
                                      .ForAllLogs()
                                      .Email()
                                          .WithRecipients(recipients)
                                          .WithSmtpClient(mailKitSmtpClient, "sender@example.com")
                                          .Add()
                                      .BuildLogger(); 
        }
        public void Adding_Email_Destination_With_Recipients_Function()
        {
            LogModelFunc<ICollection<string>> recipientsFunc = (logModel) =>
            {
                if (logModel.LogTypeIsIn(LogType.Error, LogType.Failure, LogType.Critical))
                    return new string[] { "recipient1@example.com" };
                else
                    return new string[] { "recipient2@example.com" };
            };
            var logger = LoggerBuilder.Logger()
                                      .ForAllLogs()
                                      .Email()
                                          .WithRecipients(recipientsFunc)
                                          .WithSmtpClient(mailKitSmtpClient, "sender@example.com")
                                          .Add()
                                      .BuildLogger();
        }
        public void Adding_Email_Destination_With_Custom_Recipient_Provider()
        {
            IRecipientProvider customRecipientProvider = new Mock<IRecipientProvider>().Object;
            var logger = LoggerBuilder.Logger()
                                      .ForAllLogs()
                                      .Email()
                                          .WithRecipients(customRecipientProvider)
                                          .WithSmtpClient(mailKitSmtpClient, "sender@example.com")
                                          .Add()
                                      .BuildLogger();
        }
        public void Adding_Email_Destination_With_Mailbox_Address()
        {
            MailboxAddress fromAddress = new MailboxAddress("sender", "sender@example.com");
            var logger = LoggerBuilder.Logger()
                                      .ForAllLogs()
                                      .Email()
                                          .WithRecipients("recipient@example.com")
                                          .WithSmtpClient(mailKitSmtpClient, fromAddress)
                                          .Add()
                                      .BuildLogger();
        }
        public void Adding_Email_Destination_With_Custom_Subject_Body_And_Attachment_Templates()
        {
            var logger = LoggerBuilder.Logger()
                                      .ForAllLogs()
                                      .Email()
                                          .WithRecipients("recipient@example.com")
                                          .WithSmtpClient(mailKitSmtpClient, "sender@example.com")
                                          .WithSubject("Notification from logger $Source: $LogType - $Description")
                                          .WithBody(customBodyTemplate)
                                          .WithAttachment(customAttachmentTemplate)
                                          .Add()
                                      .BuildLogger();
        }
        public void Adding_Email_Destination_With_Custom_Body_And_Attachment_Templates_And_JsonSettings()
        {
            var jsonSerializerSettings   = new JsonSerializerSettings();
            var logger = LoggerBuilder.Logger()
                                      .ForAllLogs()
                                      .Email()
                                          .WithRecipients("recipient@example.com")
                                          .WithSmtpClient(mailKitSmtpClient, "sender@example.com")
                                          .WithSubject("Notification from logger $Source: $LogType - $Description")
                                          .WithBody(customBodyTemplate, jsonSerializerSettings)
                                          .WithAttachment(customAttachmentTemplate, jsonSerializerSettings)
                                          .Add()
                                      .BuildLogger();
        }
        public void Adding_Email_Destination_With_Custom_Subject_Body_And_Attachment_Serializers()
        {
            ILogSerializer customSubjectSerializer = new Mock<ILogSerializer>().Object;
            ILogSerializer customBodySerializer = new Mock<ILogSerializer>().Object;
            ILogSerializer customAttachmentSerializer = new Mock<ILogSerializer>().Object;

            var logger = LoggerBuilder.Logger()
                                      .ForAllLogs()
                                      .Email()
                                          .WithRecipients("recipient@example.com")
                                          .WithSmtpClient(mailKitSmtpClient, "sender@example.com")
                                          .WithSubject(customSubjectSerializer)
                                          .WithBody(customBodySerializer)
                                          .WithAttachment(customAttachmentSerializer)
                                          .Add()
                                      .BuildLogger();
        } 
        public void Adding_Email_Destination_With_Custom_Subject_Text_Function()
        {
            var logger = LoggerBuilder.Logger()
                                      .ForAllLogs()
                                      .Email()
                                          .WithRecipients("recipient@example.com")
                                          .WithSmtpClient(mailKitSmtpClient, "sender@example.com")
                                          .WithSubject(m => $"Notification from logger {m.Source}: {m.LogType} - {m.Description}")
                                          .Add()
                                      .BuildLogger();
        }
    }
}
