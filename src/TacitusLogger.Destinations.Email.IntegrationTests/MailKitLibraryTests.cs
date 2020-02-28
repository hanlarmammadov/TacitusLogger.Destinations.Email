using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using netDumbster.smtp;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TacitusLogger.Destinations.Email.IntegrationTests
{
    [TestFixture]
    public class MailKitLibraryTests
    {
        private SimpleSmtpServer _fakeSmtpServer;
        private SmtpClient _smtpClient;

        private void AssertIfMailsSentSuccessfully(SimpleSmtpServer smtpServer, MailboxAddress from, string resipient, string expectedSubject, string expectedBody, string expectedAttachment)
        {
            Assert.AreEqual(1, smtpServer.ReceivedEmailCount);
            var receivedEmail = smtpServer.ReceivedEmail[0];
            Assert.AreEqual(from.Address, receivedEmail.FromAddress.Address);

            Assert.AreEqual(resipient, receivedEmail.ToAddresses[0].Address);
            Assert.AreEqual(expectedBody, receivedEmail.MessageParts[0].BodyData);
            Assert.AreEqual(expectedSubject, receivedEmail.Headers["Subject"]);
            if (expectedAttachment != null)
            {
                Assert.AreEqual(2, receivedEmail.MessageParts.Length);
                Assert.AreEqual(expectedAttachment, receivedEmail.MessageParts[1].BodyData);
                //Assert.AreEqual("application/octet-stream", receivedEmail.MessageParts[1].HeaderData);
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
        public void Sending_Simple_Mail_Using_MailKit()
        {
            // Arrange
            var message = new MimeMessage();
            var from = new MailboxAddress("sender@example.com");
            var recipient = "recipient@example.com";
            message.From.Add(from);
            message.To.Add(new MailboxAddress(recipient));
            message.Subject = "testSubject";
            message.Body = new TextPart("plain")
            {
                Text = "абвгдеёжз" + "  "
            };

            // Act
            _smtpClient.Send(message);

            // Assert
            AssertIfMailsSentSuccessfully(_fakeSmtpServer, from, recipient, "testSubject", "абвгдеёжз", null);
        }

        [Test]
        public void Sending_Mail_With_Attachment_Using_MailKit()
        {
            // Arrange
            var message = new MimeMessage();
            var from = new MailboxAddress("sender@example.com");
            var recipient = "recipient@example.com";
            message.From.Add(from);
            message.To.Add(new MailboxAddress(recipient));
            message.Subject = "testSubject";
            var textBodyPart = new TextPart("plain")
            {
                Text = "абвгдеёжз"
            };
            string attachmentString = "attachmentString";
            var bytes = Encoding.UTF8.GetBytes(attachmentString);
            var attachmentPart = new MimePart("application", "octet-stream")
            {
                Content = new MimeContent(new MemoryStream(bytes)),
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                FileName = "Log attachment.txt"
            };
            var multipart = new Multipart("mixed");
            multipart.Add(textBodyPart);
            multipart.Add(attachmentPart);
            message.Body = multipart;

            // Act
            _smtpClient.Send(message);

            // Assert
            AssertIfMailsSentSuccessfully(_fakeSmtpServer, from, recipient, "testSubject", "абвгдеёжз", attachmentString);
        }
    }
}
