using MailKit.Net.Smtp;
using MimeKit;
using netDumbster.smtp;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace TacitusLogger.Destinations.Email.IntegrationTests
{
    [TestFixture]
    public class StandardSmtpClientFacadeTests
    {
        private SimpleSmtpServer _fakeSmtpServer;
        private SmtpClient _smtpClient;

        private void AssertIfMailsSentSuccessfully(SimpleSmtpServer smtpServer, MailboxAddress from, List<string> resipients, string expectedSubject, string expectedBody, string expectedAttachment)
        {
            Assert.AreEqual(1, smtpServer.ReceivedEmailCount);
            var receivedEmail = smtpServer.ReceivedEmail[0];
            Assert.AreEqual(from.Address, receivedEmail.FromAddress.Address);
            Assert.AreEqual(resipients.Count, receivedEmail.ToAddresses.Length);

            for (int i = 0; i < resipients.Count; i++)
            {
                Assert.AreEqual(resipients[i], receivedEmail.ToAddresses[i].Address);
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

        private string GetSampleJsonString()
        {
            Object obj = new
            {
                Name1 = "Value1",
                Name2 = "Value1",
                Inner = new
                {
                    Name1 = "Value1",
                    Name2 = "Value1"
                }
            };
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
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
        public void SendEmail_When_CalledWithOneRecipient_SendsEmailSuccessfully()
        {
            //Arrange 
            StandardSmtpClientFacade standardSmtpClientFacade = new StandardSmtpClientFacade();
            MailboxAddress from = new MailboxAddress("from@example.com");
            List<string> resipients = new List<string>() { "recipient@example.com" };

            //Act
            standardSmtpClientFacade.SendEmail(_smtpClient, from, resipients, "testSubject", "testBody  ");

            //Assert 
            AssertIfMailsSentSuccessfully(_fakeSmtpServer, from, resipients, "testSubject", "testBody", null);
        }

        [Test]
        public void SendEmail_When_CalledWithSeveralRecipients_SendsEmailsSuccessfully()
        {
            //Arrange 
            StandardSmtpClientFacade standardSmtpClientFacade = new StandardSmtpClientFacade();
            MailboxAddress from = new MailboxAddress("from@example.com");
            List<string> resipients = new List<string>() { "recipient1@example.com", "recipient2@example.com", "recipient3@example.com", };

            //Act
            standardSmtpClientFacade.SendEmail(_smtpClient, from, resipients, "testSubject", "testBody");

            //Assert 
            AssertIfMailsSentSuccessfully(_fakeSmtpServer, from, resipients, "testSubject", "testBody", null);
        }

        [Test]
        public void SendEmail_When_CalledWithOneRecipientAndAttachment_SendsEmailSuccessfully()
        {
            //Arrange 
            StandardSmtpClientFacade standardSmtpClientFacade = new StandardSmtpClientFacade();
            MailboxAddress from = new MailboxAddress("from@example.com");
            List<string> resipients = new List<string>() { "recipient@example.com" };
            string attachmentString = "attachmentString";

            //Act
            standardSmtpClientFacade.SendEmail(_smtpClient, from, resipients, "testSubject", "testBody", attachmentString);

            //Assert 
            AssertIfMailsSentSuccessfully(_fakeSmtpServer, from, resipients, "testSubject", "testBody", attachmentString);
        }

        [Test]
        public void SendEmail_When_CalledWithSeveralRecipientsAndAttachment_SendsEmailSuccessfully()
        {
            //Arrange 
            StandardSmtpClientFacade standardSmtpClientFacade = new StandardSmtpClientFacade();
            MailboxAddress from = new MailboxAddress("from@example.com");
            List<string> resipients = new List<string>() { "recipient1@example.com", "recipient2@example.com", "recipient3@example.com", "recipient4@example.com" };
            string attachmentString = "attachmentString";

            //Act
            standardSmtpClientFacade.SendEmail(_smtpClient, from, resipients, "testSubject", "testBody", attachmentString);

            //Assert 
            AssertIfMailsSentSuccessfully(_fakeSmtpServer, from, resipients, "testSubject", "testBody", attachmentString);
        }

        [Test]
        public void SendEmail_When_CalledSeveralTimes_SendsEmailsSuccessfully()
        {
            //Arrange 
            StandardSmtpClientFacade standardSmtpClientFacade = new StandardSmtpClientFacade();
            MailboxAddress from = new MailboxAddress("from@example.com");
            List<string> resipients = new List<string>() { "recipient@example.com" };

            //Act
            standardSmtpClientFacade.SendEmail(_smtpClient, from, resipients, "testSubject", "testBody");
            standardSmtpClientFacade.SendEmail(_smtpClient, from, resipients, "testSubject", "testBody");
            standardSmtpClientFacade.SendEmail(_smtpClient, from, resipients, "testSubject", "testBody");

            //Assert 
            Assert.AreEqual(3, _fakeSmtpServer.ReceivedEmailCount);
        }

        [Test]
        public void SendEmail_When_CalledWithNullFromAddress_Throws_ArgumentNullException()
        {
            //Arrange 
            StandardSmtpClientFacade standardSmtpClientFacade = new StandardSmtpClientFacade();
            List<string> resipients = new List<string>() { "recipient1@example.com", "recipient2@example.com", "recipient3@example.com" };

            //Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                //Act 
                standardSmtpClientFacade.SendEmail(_smtpClient, null, resipients, "testSubject", "testBody");
            });
        }

        [Test]
        public void SendEmail_When_Called_With_Null_Recipients_List_Throws_ArgumentNullException()
        {
            //Arrange 
            StandardSmtpClientFacade standardSmtpClientFacade = new StandardSmtpClientFacade();
            MailboxAddress from = new MailboxAddress("from@example.com");

            //Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                //Act 
                standardSmtpClientFacade.SendEmail(_smtpClient, from, null, "testSubject", "testBody");
            });
        }

        [Test]
        public void SendEmail_When_Called_With_Recipients_List_That_Contains_Nulls_Throws_ArgumentException()
        {
            //Arrange 
            StandardSmtpClientFacade standardSmtpClientFacade = new StandardSmtpClientFacade();
            MailboxAddress from = new MailboxAddress("from@example.com");
            List<string> resipients = new List<string>() { "recipient1@example.com", "recipient2@example.com", "recipient3@example.com", null };

            //Assert
            Assert.Catch<ArgumentException>(() =>
            {
                //Act 
                standardSmtpClientFacade.SendEmail(_smtpClient, from, resipients, "testSubject", "testBody");
            });
        }

        [Test]
        public void SendEmail_When_Called_With_Empty_Recipients_List_Throws_ArgumentException()
        {
            //Arrange 
            StandardSmtpClientFacade standardSmtpClientFacade = new StandardSmtpClientFacade();
            MailboxAddress from = new MailboxAddress("from@example.com");
            List<string> resipients = new List<string>() { };

            //Assert
            Assert.Catch<ArgumentException>(() =>
            {
                //Act 
                standardSmtpClientFacade.SendEmail(_smtpClient, from, resipients, "testSubject", "testBody");
            });
        }

        [Test]
        public void SendEmail_When_CalledWithNullAttachment_SendsEmailsSuccessfully()
        {
            //Arrange 
            StandardSmtpClientFacade standardSmtpClientFacade = new StandardSmtpClientFacade();
            MailboxAddress from = new MailboxAddress("from@example.com");
            List<string> resipients = new List<string>() { "recipient@example.com" };

            //Act 
            standardSmtpClientFacade.SendEmail(_smtpClient, from, resipients, "testSubject", "testBody", null);
        }

        [Test]
        public void SendEmail_WhenSubjectIsJsonString_SendsEmailsSuccessfully()
        {
            //Arrange 
            StandardSmtpClientFacade standardSmtpClientFacade = new StandardSmtpClientFacade();
            MailboxAddress from = new MailboxAddress("from@example.com");
            List<string> resipients = new List<string>() { "recipient@example.com" };
            string jsonSubject = GetSampleJsonString();

            //Act 
            standardSmtpClientFacade.SendEmail(_smtpClient, from, resipients, jsonSubject, "testBody");

            //Assert
            AssertIfMailsSentSuccessfully(_fakeSmtpServer, from, resipients, jsonSubject, "testBody", null);
        }

        [Test]
        public void SendEmail_WhenBodyIsJsonString_SendsEmailsSuccessfully()
        {
            //Arrange 
            StandardSmtpClientFacade standardSmtpClientFacade = new StandardSmtpClientFacade();
            MailboxAddress from = new MailboxAddress("from@example.com");
            List<string> resipients = new List<string>() { "recipient@example.com" };
            string jsonBody = GetSampleJsonString();

            //Act 
            standardSmtpClientFacade.SendEmail(_smtpClient, from, resipients, "testSubect", jsonBody);

            //Assert
            AssertIfMailsSentSuccessfully(_fakeSmtpServer, from, resipients, "testSubect", jsonBody, null);
        }

        [Test]
        public void SendEmail_WhenAttachmentIsJsonString_SendsEmailsSuccessfully()
        {
            //Arrange 
            StandardSmtpClientFacade standardSmtpClientFacade = new StandardSmtpClientFacade();
            MailboxAddress from = new MailboxAddress("from@example.com");
            List<string> resipients = new List<string>() { "recipient@example.com" };
            string jsonAttachment = GetSampleJsonString();

            //Act 
            standardSmtpClientFacade.SendEmail(_smtpClient, from, resipients, "testSubect", "testBody", jsonAttachment);

            //Assert
            AssertIfMailsSentSuccessfully(_fakeSmtpServer, from, resipients, "testSubect", "testBody", jsonAttachment);
        }

        [Test]
        public void SendEmail_WhenAttachmentIsBinaryString_SendsEmailsSuccessfully()
        {
            //Arrange 
            StandardSmtpClientFacade standardSmtpClientFacade = new StandardSmtpClientFacade();
            MailboxAddress from = new MailboxAddress("from@example.com");
            List<string> resipients = new List<string>() { "recipient@example.com" };
            string binaryString = new Bogus.Randomizer().Utf16String(1000, 1000);

            //Act 
            standardSmtpClientFacade.SendEmail(_smtpClient, from, resipients, "testSubect", "testBody", binaryString);

            //Assert
            AssertIfMailsSentSuccessfully(_fakeSmtpServer, from, resipients, "testSubect", "testBody", binaryString);
        }

        [Test]
        public void SendEmail_WhenAttachmentIsHugeBinaryString_SendsEmailsSuccessfully()
        {
            //Arrange 
            StandardSmtpClientFacade standardSmtpClientFacade = new StandardSmtpClientFacade();
            MailboxAddress from = new MailboxAddress("from@example.com");
            List<string> resipients = new List<string>() { "recipient@example.com" };
            string binaryString = new Bogus.Randomizer().Utf16String(10_000_000, 10_000_000);

            //Act 
            standardSmtpClientFacade.SendEmail(_smtpClient, from, resipients, "testSubect", "testBody", binaryString);

            //Assert
            AssertIfMailsSentSuccessfully(_fakeSmtpServer, from, resipients, "testSubect", "testBody", binaryString);
        }

        [Test]
        public void SendEmail_WhenBodyIsHugeTestString_SendsEmailsSuccessfully()
        {
            //Arrange 
            StandardSmtpClientFacade standardSmtpClientFacade = new StandardSmtpClientFacade();
            MailboxAddress from = new MailboxAddress("from@example.com");
            List<string> resipients = new List<string>() { "recipient@example.com" };
            string hugeTextBody = new Bogus.Randomizer().String2(10_000_000);

            //Act 
            standardSmtpClientFacade.SendEmail(_smtpClient, from, resipients, "testSubect", hugeTextBody, null);

            //Assert
            AssertIfMailsSentSuccessfully(_fakeSmtpServer, from, resipients, "testSubect", hugeTextBody, null);
        }
    }
}
