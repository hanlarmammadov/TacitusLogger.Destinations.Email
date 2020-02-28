using MailKit.Net.Smtp;
using MimeKit;
using Moq;
using netDumbster.smtp;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TacitusLogger.Builders;
using TacitusLogger.Serializers;

namespace TacitusLogger.Destinations.Email.IntegrationTests
{
    [TestFixture]
    public class EmailDestinationTests
    {
        private SimpleSmtpServer _fakeSmtpServer;
        private SmtpClient _smtpClient;

        private void AssertIfMailsSentSuccessfully(SimpleSmtpServer smtpServer, MailboxAddress from, List<string> recipients, string expectedSubject, string expectedBody, string expectedAttachment)
        {
            Assert.AreEqual(1, smtpServer.ReceivedEmailCount);
            var receivedEmail = smtpServer.ReceivedEmail[0];
            Assert.AreEqual(from.Address, receivedEmail.FromAddress.Address);
            Assert.AreEqual(recipients.Count, receivedEmail.ToAddresses.Length);

            for (int i = 0; i < recipients.Count; i++)
            {
                Assert.AreEqual(recipients[i], receivedEmail.ToAddresses[i].Address);
                Assert.AreEqual(expectedBody, receivedEmail.MessageParts[0].BodyData);
                Assert.AreEqual(expectedSubject, receivedEmail.Headers["Subject"]);
                if (expectedAttachment != null)
                {
                    Assert.AreEqual(2, receivedEmail.MessageParts.Length);
                    Assert.AreEqual(expectedAttachment, receivedEmail.MessageParts[1].BodyData);
                    Assert.AreEqual("application/octet-stream", receivedEmail.MessageParts[1].HeaderData);
                }
            }
        }

        [SetUp]
        public void SetUp()
        {
            _fakeSmtpServer = SimpleSmtpServer.Start(5555);
            _smtpClient = new SmtpClient();
            _smtpClient.Connect("localhost", _fakeSmtpServer.Configuration.Port, false); 
        }

        [TearDown]
        public void TearDown()
        {
            _fakeSmtpServer.Stop();
            _smtpClient.Disconnect(true);
            _smtpClient.Dispose();
        }

        [Test]
        public void Logger_That_Contains_One_Log_Group_For_All_Logs_With_One_Email_Destination_In_It()
        {
            // Setup log serializers for subject, body and attachment to return predefined strings.
            var subjectLogSerializerMock = new Mock<ILogSerializer>();
            subjectLogSerializerMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns("Subject");
            var bodyLogSerializerMock = new Mock<ILogSerializer>();
            bodyLogSerializerMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns("Body");
            var attachmentLogSerializerMock = new Mock<ILogSerializer>();
            attachmentLogSerializerMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns("Attachment");
            var recipientProviderMock = new Mock<IRecipientProvider>();
            string[] recipients = new string[] { "recipient1@example.com", "recipient2@example.com", "recipient3@example.com" };
            recipientProviderMock.Setup(x => x.GetRecipients(It.IsAny<LogModel>())).Returns(recipients);
            // Build the logger.
            ILogger logger = LoggerBuilder.Logger("App1").ForAllLogs().Email()
                                                                      .WithSmtpClient(_smtpClient, "sender@example.com")
                                                                      .WithRecipients(recipientProviderMock.Object)
                                                                      .WithSubject(subjectLogSerializerMock.Object)
                                                                      .WithBody(bodyLogSerializerMock.Object)
                                                                      .WithAttachment(attachmentLogSerializerMock.Object)
                                                                      .Add()
                                                          .BuildLogger();
            var loggingObject = new { Name = "Value" };

            // Act
            var logId = logger.LogFailure("Context1", "Description1", loggingObject);

            // Assert
            AssertIfMailsSentSuccessfully(_fakeSmtpServer, new MailboxAddress("sender@example.com"), recipients.ToList(), "Subject", "Body", "Attachment");
        }

        [Test]
        public void Logger_That_Contains_One_Log_Group_For_All_Logs_With_One_Email_Destination_With_Default_Settings()
        {
            // Build the logger.
            ILogger logger = LoggerBuilder.Logger("App1").ForAllLogs().Email()
                                                                      .WithSmtpClient(_smtpClient, "sender@example.com")
                                                                      .WithRecipients("recipient@example.com")
                                                                      .Add()
                                                          .BuildLogger();
            var loggingObject = new { Name = "Value" };

            // Act
            var logId = logger.LogFailure("Context1", "Description1", loggingObject);

            // Assert
            var receivedEmail = _fakeSmtpServer.ReceivedEmail[0];
            Assert.IsNotEmpty(receivedEmail.MessageParts[0].BodyData);
            Assert.IsNotEmpty(receivedEmail.Headers["Subject"]);
            // No attachments
            Assert.AreEqual(1, receivedEmail.MessageParts.Length);
        }

        [Test]
        public void Logger_That_Contains_One_Log_Group_For_All_Logs_With_One_Email_Destination_Sending_To_Several_Recipients()
        {
            // Build the logger.
            string[] recipients = new string[] { "recipient1@example.com", "recipient2@example.com", "recipient3@example.com" };
            ILogger logger = LoggerBuilder.Logger("App1").ForAllLogs().Email()
                                                                      .WithSmtpClient(_smtpClient, "sender@example.com")
                                                                      .WithRecipients(recipients)
                                                                      .Add()
                                                          .BuildLogger();
            var loggingObject = new { Name = "Value" };

            // Act
            var logId = logger.LogFailure("Context1", "Description1", loggingObject);

            // Assert
            Assert.AreEqual(1, _fakeSmtpServer.ReceivedEmailCount);
            var receivedEmail = _fakeSmtpServer.ReceivedEmail[0];
            Assert.AreEqual(recipients.Length, receivedEmail.ToAddresses.Length);
            for (int i = 0; i < recipients.Length; i++)
                Assert.AreEqual(recipients[i], receivedEmail.ToAddresses[i].Address);
        }

    }
}
