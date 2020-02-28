using MailKit.Net.Smtp;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using TacitusLogger.Builders;
using TacitusLogger.Serializers;

namespace TacitusLogger.Destinations.Email.IntegrationTests
{
    [TestFixture]
    public class EmailDestinationBuilderTests
    {
        [Test]
        public void LoggerBuilder_With_One_Log_Group_For_All_Logs_With_One_Email_Destination_In_It()
        {
            // Setup log serializers for subject, body and attachment to return predefined strings.
            var subjectLogSerializer = new Mock<ILogSerializer>().Object;
            var bodyLogSerializer = new Mock<ILogSerializer>().Object;
            var attachmentLogSerializer = new Mock<ILogSerializer>().Object;
            var recipientProvider = new Mock<IRecipientProvider>().Object;
            SmtpClient smtpClient = new SmtpClient();
            // Build the logger.
            Logger logger = (Logger)LoggerBuilder.Logger("App1").NewLogGroup("logGroup1")
                                                                .ForAllLogs()
                                                                        .Email()
                                                                        .WithSmtpClient(smtpClient, "sender@example.com")
                                                                        .WithRecipients(recipientProvider)
                                                                        .WithSubject(subjectLogSerializer)
                                                                        .WithBody(bodyLogSerializer)
                                                                        .WithAttachment(attachmentLogSerializer)
                                                                        .Add()
                                                                .BuildLogger();
            // Assert
            var emailDestination = (EmailDestination)((LogGroup)logger.GetLogGroup("logGroup1")).LogDestinations.Single();
            Assert.AreEqual(subjectLogSerializer, emailDestination.MailSubjectGenerator);
            Assert.AreEqual(bodyLogSerializer, emailDestination.MailBodyGenerator);
            Assert.AreEqual(attachmentLogSerializer, emailDestination.MailAttachmentGenerator);
            Assert.AreEqual(smtpClient, emailDestination.SmtpClient);
            Assert.AreEqual("sender@example.com", emailDestination.From.Address);
            Assert.IsInstanceOf<StandardSmtpClientFacade>(emailDestination.SmtpClientFacade);
            Assert.AreEqual(recipientProvider, emailDestination.RecipientProvider);
        }

        [Test]
        public void LoggerBuilder_With_Email_Destination_Builder_Missing_WithSmtpClient_Setup_Method_Throws_InvalidOperationException()
        {
            Assert.Catch<InvalidOperationException>(() =>
            {
                // Try to build the logger.
                Logger logger = (Logger)LoggerBuilder.Logger("App1").NewLogGroup("logGroup1")
                                                                    .ForAllLogs()
                                                                            .Email()
                                                                            .WithRecipients("recipient@example.com")
                                                                            .Add()
                                                                     .BuildLogger();
            });
        }

        [Test]
        public void LoggerBuilder_With_Email_Destination_Builder_Missing_WithRecipients_Setup_Method_Throws_InvalidOperationException()
        {
            Assert.Catch<InvalidOperationException>(() =>
            {
                // Try to build the logger.
                Logger logger = (Logger)LoggerBuilder.Logger("App1").NewLogGroup("logGroup1")
                                                                    .ForAllLogs()
                                                                            .Email()
                                                                            .WithSmtpClient(new SmtpClient(), "sender@example.com")
                                                                            .Add()
                                                                     .BuildLogger();
            });
        }

        [Test]
        public void LoggerBuilder_With_Email_Destination_Builder_Missing_WithSubject_And_WithBody_And_WithAttachment_Uses_Defaults()
        {
            // Build the logger.
            Logger logger = (Logger)LoggerBuilder.Logger("App1").NewLogGroup("logGroup1")
                                                                .ForAllLogs()
                                                                        .Email()
                                                                        .WithSmtpClient(new SmtpClient(), "sender@example.com")
                                                                        .WithRecipients("recipient@example.com")
                                                                        .Add()
                                                                 .BuildLogger();
            // Assert
            var emailDestination = (EmailDestination)((LogGroup)logger.GetLogGroup("logGroup1")).LogDestinations.Single();
            Assert.IsInstanceOf<SimpleTemplateLogSerializer>(emailDestination.MailSubjectGenerator);
            Assert.IsInstanceOf<ExtendedTemplateLogSerializer>(emailDestination.MailBodyGenerator);
            Assert.IsNull(emailDestination.MailAttachmentGenerator);
        }
    }
}
