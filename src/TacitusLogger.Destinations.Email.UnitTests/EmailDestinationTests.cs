using MailKit.Net.Smtp;
using MimeKit;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TacitusLogger.Serializers;

namespace TacitusLogger.Destinations.Email.UnitTests
{
    [TestFixture]
    public class EmailDestinationTests
    {
        #region Ctor tests

        [Test]
        public void Ctor_Taking_All_Params_When_Called_Sets_All_Dependencies()
        {
            //Arrange
            SmtpClient smtpClient = new SmtpClient(); 
            MailboxAddress fromAddress = new MailboxAddress("recipient@example.com");
            var recipientProvider = new Mock<IRecipientProvider>().Object;
            var mailSubjectGenerator = new Mock<ILogSerializer>().Object;
            var mailBodyGenerator = new Mock<ILogSerializer>().Object;
            var mailAttachmentGenerator = new Mock<ILogSerializer>().Object;

            //Act
            EmailDestination emailDestination = new EmailDestination(smtpClient, fromAddress, recipientProvider, mailSubjectGenerator, mailBodyGenerator, mailAttachmentGenerator);

            //Assert
            Assert.AreEqual(smtpClient, emailDestination.SmtpClient);
            Assert.AreEqual(fromAddress, emailDestination.From);
            Assert.AreEqual(recipientProvider, emailDestination.RecipientProvider);
            Assert.AreEqual(mailSubjectGenerator, emailDestination.MailSubjectGenerator);
            Assert.AreEqual(mailBodyGenerator, emailDestination.MailBodyGenerator);
            Assert.AreEqual(mailAttachmentGenerator, emailDestination.MailAttachmentGenerator);
            Assert.IsInstanceOf<StandardSmtpClientFacade>(emailDestination.SmtpClientFacade);
        }
        [Test]
        public void Ctor_Taking_All_Params_When_Called_With_Null_Smtp_Client_Throws_ArgumentNullException()
        {
            //Arrange 
            MailboxAddress fromAddress = new MailboxAddress("recipient@example.com");
            var recipientProvider = new Mock<IRecipientProvider>().Object;
            var mailSubjectGenerator = new Mock<ILogSerializer>().Object;
            var mailBodyGenerator = new Mock<ILogSerializer>().Object;
            var mailAttachmentGenerator = new Mock<ILogSerializer>().Object;

            //Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                //Act
                EmailDestination emailDestination = new EmailDestination(null, fromAddress, recipientProvider, mailSubjectGenerator, mailBodyGenerator, mailAttachmentGenerator);
            });
        }
        [Test]
        public void Ctor_Taking_All_Params_When_Called_With_Null_From_Address_Throws_ArgumentNullException()
        {
            //Arrange 
            SmtpClient smtpClient = new SmtpClient();
            var recipientProvider = new Mock<IRecipientProvider>().Object;
            var mailSubjectGenerator = new Mock<ILogSerializer>().Object;
            var mailBodyGenerator = new Mock<ILogSerializer>().Object;
            var mailAttachmentGenerator = new Mock<ILogSerializer>().Object;

            //Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                //Act
                EmailDestination emailDestination = new EmailDestination(smtpClient, null, recipientProvider, mailSubjectGenerator, mailBodyGenerator, mailAttachmentGenerator);
            });
        }
        [Test]
        public void Ctor_Taking_All_Params_When_Called_With_Null_RecipientProvider_Throws_ArgumentNullException()
        {
            //Arrange 
            SmtpClient smtpClient = new SmtpClient();
            MailboxAddress fromAddress = new MailboxAddress("recipient@example.com");
            var mailSubjectGenerator = new Mock<ILogSerializer>().Object;
            var mailBodyGenerator = new Mock<ILogSerializer>().Object;
            var mailAttachmentGenerator = new Mock<ILogSerializer>().Object;

            //Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                //Act
                EmailDestination emailDestination = new EmailDestination(smtpClient, fromAddress, null, mailSubjectGenerator, mailBodyGenerator, mailAttachmentGenerator);
            });
        }
        [Test]
        public void Ctor_Taking_All_Params_When_Called_With_Null_Subject_Generator_Sets_Subject_Generator_As_Null()
        {
            //Arrange
            SmtpClient smtpClient = new SmtpClient();
            MailboxAddress fromAddress = new MailboxAddress("recipient@example.com");
            var recipientProvider = new Mock<IRecipientProvider>().Object;
            var mailBodyGenerator = new Mock<ILogSerializer>().Object;
            var mailAttachmentGenerator = new Mock<ILogSerializer>().Object;

            //Act
            EmailDestination emailDestination = new EmailDestination(smtpClient, fromAddress, recipientProvider, null, mailBodyGenerator, mailAttachmentGenerator);

            //Assert 
            Assert.AreEqual(null, emailDestination.MailSubjectGenerator); 
        }
        [Test]
        public void Ctor_Taking_All_Params_When_Called_With_Null_Body_Generator_Sets_Body_Generator_As_Null()
        {
            //Arrange
            SmtpClient smtpClient = new SmtpClient();
            MailboxAddress fromAddress = new MailboxAddress("recipient@example.com");
            var recipientProvider = new Mock<IRecipientProvider>().Object;
            var mailSubjectGenerator = new Mock<ILogSerializer>().Object;
            var mailAttachmentGenerator = new Mock<ILogSerializer>().Object;

            //Act
            EmailDestination emailDestination = new EmailDestination(smtpClient, fromAddress, recipientProvider, mailSubjectGenerator, null, mailAttachmentGenerator);

            //Assert
            Assert.AreEqual(smtpClient, emailDestination.SmtpClient);
            Assert.AreEqual(fromAddress, emailDestination.From);
            Assert.AreEqual(recipientProvider, emailDestination.RecipientProvider);
            Assert.AreEqual(mailSubjectGenerator, emailDestination.MailSubjectGenerator);
            Assert.AreEqual(null, emailDestination.MailBodyGenerator);
            Assert.AreEqual(mailAttachmentGenerator, emailDestination.MailAttachmentGenerator);
        }
        [Test]
        public void Ctor_Taking_All_Params_When_Called_With_Null_Attachment_Generator_Sets_Attachment_Generator_As_Null()
        {
            //Arrange
            SmtpClient smtpClient = new SmtpClient();
            MailboxAddress fromAddress = new MailboxAddress("recipient@example.com");
            var recipientProvider = new Mock<IRecipientProvider>().Object;
            var mailSubjectGenerator = new Mock<ILogSerializer>().Object;
            var mailBodyGenerator = new Mock<ILogSerializer>().Object;

            //Act
            EmailDestination emailDestination = new EmailDestination(smtpClient, fromAddress, recipientProvider, mailSubjectGenerator, mailBodyGenerator, null);

            //Assert
            Assert.AreEqual(smtpClient, emailDestination.SmtpClient);
            Assert.AreEqual(fromAddress, emailDestination.From);
            Assert.AreEqual(recipientProvider, emailDestination.RecipientProvider);
            Assert.AreEqual(mailSubjectGenerator, emailDestination.MailSubjectGenerator);
            Assert.AreEqual(mailBodyGenerator, emailDestination.MailBodyGenerator);
            Assert.AreEqual(null, emailDestination.MailAttachmentGenerator);
        }

        [Test]
        public void Ctor_Taking_SmtpClient_And_FromAddress_And_RecipientsList_When_Called_Sets_Defaults_Correctly()
        {
            //Arrange
            SmtpClient smtpClient = new SmtpClient();
            MailboxAddress fromAddress = new MailboxAddress("recipient@example.com");
            string[] recipientsList = new string[] { "recipients@example.com" };


            //Act
            EmailDestination emailDestination = new EmailDestination(smtpClient, fromAddress, recipientsList);

            //Assert
            Assert.AreEqual(smtpClient, emailDestination.SmtpClient);
            Assert.AreEqual(fromAddress, emailDestination.From);
            Assert.IsInstanceOf<EmailListRecipientProvider>(emailDestination.RecipientProvider);
            Assert.IsInstanceOf<SimpleTemplateLogSerializer>(emailDestination.MailSubjectGenerator);
            Assert.IsInstanceOf<ExtendedTemplateLogSerializer>(emailDestination.MailBodyGenerator);
            Assert.IsNull(emailDestination.MailAttachmentGenerator);
            Assert.IsInstanceOf<StandardSmtpClientFacade>(emailDestination.SmtpClientFacade);
        }
        [Test]
        public void Ctor_Taking_SmtpClient_And_FromAddress_And_RecipientsList_When_Called_With_Null_SmtpClient_Throws_ArgumentNullException()
        {
            //Arrange 
            MailboxAddress fromAddress = new MailboxAddress("recipient@example.com");
            string[] recipientsList = new string[] { "recipients@example.com" };

            Assert.Catch<ArgumentNullException>(() =>
            {
                //Act
                EmailDestination emailDestination = new EmailDestination(null, fromAddress, recipientsList);
            });
        }
        [Test]
        public void Ctor_Taking_SmtpClient_And_FromAddress_And_RecipientsList_When_Called_With_Null_FromAddress_Throws_ArgumentNullException()
        {
            //Arrange
            SmtpClient smtpClient = new SmtpClient();
            string[] recipientsList = new string[] { "recipients@example.com" };

            Assert.Catch<ArgumentNullException>(() =>
            {
                //Act
                EmailDestination emailDestination = new EmailDestination(smtpClient, null, recipientsList);
            });
        }
        [Test]
        public void Ctor_Taking_SmtpClient_And_FromAddress_And_RecipientsList_When_Called_With_Null_RecipientsList_Throws_ArgumentNullException()
        {
            //Arrange
            SmtpClient smtpClient = new SmtpClient();
            MailboxAddress fromAddress = new MailboxAddress("recipient@example.com");
            string[] recipientsList = null;

            Assert.Catch<ArgumentNullException>(() =>
            {
                //Act
                EmailDestination emailDestination = new EmailDestination(smtpClient, fromAddress, recipientsList);
            });
        }

        [Test]
        public void Ctor_Taking_Smtp_Client_From_Address_Recipient_Provider_When_Called_Sets_Dependencies_Correctly()
        {
            //Arrange
            SmtpClient smtpClient = new SmtpClient();
            MailboxAddress fromAddress = new MailboxAddress("recipient@example.com");
            IRecipientProvider recipientProvider = new Mock<IRecipientProvider>().Object;
             
            //Act
            EmailDestination emailDestination = new EmailDestination(smtpClient, fromAddress, recipientProvider);

            //Assert
            Assert.AreEqual(smtpClient, emailDestination.SmtpClient);
            Assert.AreEqual(fromAddress, emailDestination.From);
            Assert.AreEqual(recipientProvider, emailDestination.RecipientProvider); 
            Assert.IsInstanceOf<SimpleTemplateLogSerializer>(emailDestination.MailSubjectGenerator);
            Assert.IsInstanceOf<ExtendedTemplateLogSerializer>(emailDestination.MailBodyGenerator);
            Assert.IsNull(emailDestination.MailAttachmentGenerator);
            Assert.IsInstanceOf<StandardSmtpClientFacade>(emailDestination.SmtpClientFacade);
        }

        [Test]
        public void Ctor_Taking_Smtp_Client_From_Address_Recipient_Provider_Mail_Subject_Generator_When_Called_Sets_Dependencies_Correctly()
        {
            //Arrange
            SmtpClient smtpClient = new SmtpClient();
            MailboxAddress fromAddress = new MailboxAddress("recipient@example.com");
            IRecipientProvider recipientProvider = new Mock<IRecipientProvider>().Object;
            ILogSerializer mailSubjectGenerator = new Mock<ILogSerializer>().Object;

            //Act
            EmailDestination emailDestination = new EmailDestination(smtpClient, fromAddress, recipientProvider, mailSubjectGenerator);

            //Assert
            Assert.AreEqual(smtpClient, emailDestination.SmtpClient);
            Assert.AreEqual(fromAddress, emailDestination.From);
            Assert.AreEqual(recipientProvider, emailDestination.RecipientProvider);
            Assert.AreEqual(mailSubjectGenerator, emailDestination.MailSubjectGenerator);
            Assert.IsInstanceOf<ExtendedTemplateLogSerializer>(emailDestination.MailBodyGenerator);
            Assert.IsNull(emailDestination.MailAttachmentGenerator);
            Assert.IsInstanceOf<StandardSmtpClientFacade>(emailDestination.SmtpClientFacade);
        }

        [Test]
        public void Ctor_Taking_Smtp_Client_From_Address_Recipient_Provider_Mail_Subject_Generator_Mail_Body_Generator_When_Called_Sets_Dependencies_Correctly()
        {
            //Arrange
            SmtpClient smtpClient = new SmtpClient();
            MailboxAddress fromAddress = new MailboxAddress("recipient@example.com");
            IRecipientProvider recipientProvider = new Mock<IRecipientProvider>().Object;
            ILogSerializer mailSubjectGenerator = new Mock<ILogSerializer>().Object;
            ILogSerializer mailBodyGenerator = new Mock<ILogSerializer>().Object;

            //Act
            EmailDestination emailDestination = new EmailDestination(smtpClient, fromAddress, recipientProvider, mailSubjectGenerator, mailBodyGenerator);

            //Assert
            Assert.AreEqual(smtpClient, emailDestination.SmtpClient);
            Assert.AreEqual(fromAddress, emailDestination.From);
            Assert.AreEqual(recipientProvider, emailDestination.RecipientProvider);
            Assert.AreEqual(mailSubjectGenerator, emailDestination.MailSubjectGenerator);
            Assert.AreEqual(mailBodyGenerator, emailDestination.MailBodyGenerator);
            Assert.IsNull(emailDestination.MailAttachmentGenerator);
            Assert.IsInstanceOf<StandardSmtpClientFacade>(emailDestination.SmtpClientFacade);
        }

        #endregion

        #region Tests for Send method

        [Test]
        public void Send_When_Called_Calls_Recipient_Provider_And_Email_Parts_Providers_Passing_To_Them_Same_LogModel()
        {
            //Arrange
            SmtpClient smtpClient = new SmtpClient();
            MailboxAddress fromAddress = new MailboxAddress("recipient@example.com");
            var recipientProviderMock = new Mock<IRecipientProvider>();
            var mailSubjectGeneratorMock = new Mock<ILogSerializer>();
            var mailBodyGeneratorMock = new Mock<ILogSerializer>();
            var mailAttachmentGeneratorMock = new Mock<ILogSerializer>();
            EmailDestination emailDestination = new EmailDestination(smtpClient, fromAddress, recipientProviderMock.Object, mailSubjectGeneratorMock.Object, mailBodyGeneratorMock.Object, mailAttachmentGeneratorMock.Object);
            var smtpClientFacadeMock = new Mock<ISmtpClientFacade>();
            emailDestination.ResetSmtpClientFacade(smtpClientFacadeMock.Object);
            var logData = new LogModel();

            //Act
            emailDestination.Send(new LogModel[] { logData });

            //Assert
            recipientProviderMock.Verify(x => x.GetRecipients(logData), Times.Once);
            mailSubjectGeneratorMock.Verify(x => x.Serialize(logData), Times.Once);
            mailBodyGeneratorMock.Verify(x => x.Serialize(logData), Times.Once);
            mailAttachmentGeneratorMock.Verify(x => x.Serialize(logData), Times.Once);
        }
        [Test]
        public void Send_When_Called_Prepares_And_Sends_All_Mail_Subjects_To_SmtpClientFacade()
        {
            //Arrange
            SmtpClient smtpClient = new SmtpClient();
            MailboxAddress fromAddress = new MailboxAddress("recipient@example.com");
            var recipientProviderMock = new Mock<IRecipientProvider>();
            var recipients = new List<string>() { "recipient1@example.com" };
            recipientProviderMock.Setup(x => x.GetRecipients(It.IsAny<LogModel>())).Returns(recipients);
            var mailSubjectGeneratorMock = new Mock<ILogSerializer>();
            mailSubjectGeneratorMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns("mailSubject");
            var mailBodyGeneratorMock = new Mock<ILogSerializer>();
            mailBodyGeneratorMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns("mailBody");
            var mailAttachmentGeneratorMock = new Mock<ILogSerializer>();
            mailAttachmentGeneratorMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns("mailAtachment");
            EmailDestination emailDestination = new EmailDestination(smtpClient, fromAddress, recipientProviderMock.Object, mailSubjectGeneratorMock.Object, mailBodyGeneratorMock.Object, mailAttachmentGeneratorMock.Object);
            var smtpClientFacadeMock = new Mock<ISmtpClientFacade>();
            emailDestination.ResetSmtpClientFacade(smtpClientFacadeMock.Object);

            //Act
            emailDestination.Send(new LogModel[] { new LogModel() });

            //Assert
            smtpClientFacadeMock.Verify(x => x.SendEmail(smtpClient, fromAddress, recipients, "mailSubject", "mailBody", "mailAtachment"), Times.Once);
        }
        [Test]
        public void Send_When_Called_Given_That_SubjectGenerator_Is_Null_Sends_Null_Subject_To_The_SmtpClientFacade()
        {
            //Arrange
            SmtpClient smtpClient = new SmtpClient();
            MailboxAddress fromAddress = new MailboxAddress("recipient@example.com");
            var recipientProviderMock = new Mock<IRecipientProvider>();
            var recipients = new List<string>() { "recipient1@example.com" };
            recipientProviderMock.Setup(x => x.GetRecipients(It.IsAny<LogModel>())).Returns(recipients);
            var mailBodyGeneratorMock = new Mock<ILogSerializer>();
            mailBodyGeneratorMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns("mailBody");
            var mailAttachmentGeneratorMock = new Mock<ILogSerializer>();
            mailAttachmentGeneratorMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns("mailAtachment");
            EmailDestination emailDestination = new EmailDestination(smtpClient, fromAddress, recipientProviderMock.Object, null, mailBodyGeneratorMock.Object, mailAttachmentGeneratorMock.Object);
            var smtpClientFacadeMock = new Mock<ISmtpClientFacade>();
            emailDestination.ResetSmtpClientFacade(smtpClientFacadeMock.Object);

            //Act
            emailDestination.Send(new LogModel[] { new LogModel() });

            //Assert
            smtpClientFacadeMock.Verify(x => x.SendEmail(smtpClient, fromAddress, recipients, null, "mailBody", "mailAtachment"), Times.Once);
        }
        [Test]
        public void Send_When_Called_Given_That_BodyGenerator_Is_Null_Sends_Null_Body_To_The_SmtpClientFacade()
        {
            //Arrange
            SmtpClient smtpClient = new SmtpClient();
            MailboxAddress fromAddress = new MailboxAddress("recipient@example.com");
            var recipientProviderMock = new Mock<IRecipientProvider>();
            var recipients = new List<string>() { "recipient1@example.com" };
            recipientProviderMock.Setup(x => x.GetRecipients(It.IsAny<LogModel>())).Returns(recipients);
            var mailSubjectGeneratorMock = new Mock<ILogSerializer>();
            mailSubjectGeneratorMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns("mailSubject");
            var mailAttachmentGeneratorMock = new Mock<ILogSerializer>();
            mailAttachmentGeneratorMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns("mailAtachment");
            EmailDestination emailDestination = new EmailDestination(smtpClient, fromAddress, recipientProviderMock.Object, mailSubjectGeneratorMock.Object, null, mailAttachmentGeneratorMock.Object);
            var smtpClientFacadeMock = new Mock<ISmtpClientFacade>();
            emailDestination.ResetSmtpClientFacade(smtpClientFacadeMock.Object);

            //Act
            emailDestination.Send(new LogModel[] { new LogModel() });

            //Assert
            smtpClientFacadeMock.Verify(x => x.SendEmail(smtpClient, fromAddress, recipients, "mailSubject", null, "mailAtachment"), Times.Once);
        }
        [Test]
        public void Send_When_Called_Given_That_AttachmentGenerator_Is_Null_Sends_Null_Attachment_To_The_SmtpClientFacade()
        {
            //Arrange
            SmtpClient smtpClient = new SmtpClient();
            MailboxAddress fromAddress = new MailboxAddress("recipient@example.com");
            var recipientProviderMock = new Mock<IRecipientProvider>();
            var recipients = new List<string>() { "recipient1@example.com" };
            recipientProviderMock.Setup(x => x.GetRecipients(It.IsAny<LogModel>())).Returns(recipients);
            var mailSubjectGeneratorMock = new Mock<ILogSerializer>();
            mailSubjectGeneratorMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns("mailSubject");
            var mailBodyGeneratorMock = new Mock<ILogSerializer>();
            mailBodyGeneratorMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns("mailBody");
            EmailDestination emailDestination = new EmailDestination(smtpClient, fromAddress, recipientProviderMock.Object, mailSubjectGeneratorMock.Object, mailBodyGeneratorMock.Object, null);
            var smtpClientFacadeMock = new Mock<ISmtpClientFacade>();
            emailDestination.ResetSmtpClientFacade(smtpClientFacadeMock.Object);

            //Act
            emailDestination.Send(new LogModel[] { new LogModel() });

            //Assert
            smtpClientFacadeMock.Verify(x => x.SendEmail(smtpClient, fromAddress, recipients, "mailSubject", "mailBody", null), Times.Once);
        }

        #endregion

        #region Tests for SendAsync method

        [Test]
        public async Task SendAsync_When_Called_Calls_RecipientProvider_And_Email_Parts_Providers_Passing_To_Them_Same_LogModel()
        {
            //Arrange
            SmtpClient smtpClient = new SmtpClient();
            MailboxAddress fromAddress = new MailboxAddress("recipient@example.com");
            var recipientProviderMock = new Mock<IRecipientProvider>();
            var mailSubjectGeneratorMock = new Mock<ILogSerializer>();
            var mailBodyGeneratorMock = new Mock<ILogSerializer>();
            var mailAttachmentGeneratorMock = new Mock<ILogSerializer>();
            EmailDestination emailDestination = new EmailDestination(smtpClient, fromAddress, recipientProviderMock.Object, mailSubjectGeneratorMock.Object, mailBodyGeneratorMock.Object, mailAttachmentGeneratorMock.Object);
            var smtpClientFacadeMock = new Mock<ISmtpClientFacade>();
            emailDestination.ResetSmtpClientFacade(smtpClientFacadeMock.Object);
            var logData = new LogModel();
            CancellationToken cancellationToken = new CancellationToken();

            //Act
            await emailDestination.SendAsync(new LogModel[] { logData }, cancellationToken);

            //Assert
            recipientProviderMock.Verify(x => x.GetRecipientsAsync(logData, cancellationToken), Times.Once);
            mailSubjectGeneratorMock.Verify(x => x.Serialize(logData), Times.Once);
            mailBodyGeneratorMock.Verify(x => x.Serialize(logData), Times.Once);
            mailAttachmentGeneratorMock.Verify(x => x.Serialize(logData), Times.Once);
        }
        [Test]
        public async Task SendAsync_When_Called_Prepares_And_Sends_All_Mail_Subjects_To_SmtpClientFacade()
        {
            //Arrange
            SmtpClient smtpClient = new SmtpClient();
            MailboxAddress fromAddress = new MailboxAddress("recipient@example.com");
            var recipientProviderMock = new Mock<IRecipientProvider>();
            var recipients = new List<string>() { "recipient1@example.com" };
            recipientProviderMock.Setup(x => x.GetRecipientsAsync(It.IsAny<LogModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(recipients);
            var mailSubjectGeneratorMock = new Mock<ILogSerializer>();
            mailSubjectGeneratorMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns("mailSubject");
            var mailBodyGeneratorMock = new Mock<ILogSerializer>();
            mailBodyGeneratorMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns("mailBody");
            var mailAttachmentGeneratorMock = new Mock<ILogSerializer>();
            mailAttachmentGeneratorMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns("mailAtachment");
            EmailDestination emailDestination = new EmailDestination(smtpClient, fromAddress, recipientProviderMock.Object, mailSubjectGeneratorMock.Object, mailBodyGeneratorMock.Object, mailAttachmentGeneratorMock.Object);
            var smtpClientFacadeMock = new Mock<ISmtpClientFacade>();
            emailDestination.ResetSmtpClientFacade(smtpClientFacadeMock.Object); 
            CancellationToken cancellationToken = new CancellationToken();

            //Act
            await emailDestination.SendAsync(new LogModel[] { new LogModel() }, cancellationToken);

            //Assert
            smtpClientFacadeMock.Verify(x => x.SendEmailAsync(smtpClient, fromAddress, recipients, "mailSubject", "mailBody", "mailAtachment", cancellationToken), Times.Once);
        }
        [Test]
        public async Task SendAsync_When_Called_Given_That_SubjectGenerator_Is_Null_Sends_Null_Subject_To_The_SmtpClientFacade()
        {
            //Arrange
            SmtpClient smtpClient = new SmtpClient();
            MailboxAddress fromAddress = new MailboxAddress("recipient@example.com");
            var recipientProviderMock = new Mock<IRecipientProvider>();
            var recipients = new List<string>() { "recipient1@example.com" };
            recipientProviderMock.Setup(x => x.GetRecipientsAsync(It.IsAny<LogModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(recipients);
            var mailBodyGeneratorMock = new Mock<ILogSerializer>();
            mailBodyGeneratorMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns("mailBody");
            var mailAttachmentGeneratorMock = new Mock<ILogSerializer>();
            mailAttachmentGeneratorMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns("mailAtachment");
            EmailDestination emailDestination = new EmailDestination(smtpClient, fromAddress, recipientProviderMock.Object, null, mailBodyGeneratorMock.Object, mailAttachmentGeneratorMock.Object);
            var smtpClientFacadeMock = new Mock<ISmtpClientFacade>();
            emailDestination.ResetSmtpClientFacade(smtpClientFacadeMock.Object);
            CancellationToken cancellationToken = new CancellationToken();

            //Act
            await emailDestination.SendAsync(new LogModel[] { new LogModel() });

            //Assert
            smtpClientFacadeMock.Verify(x => x.SendEmailAsync(smtpClient, fromAddress, recipients, null, "mailBody", "mailAtachment", cancellationToken), Times.Once);
        }
        [Test]
        public async Task SendAsync_When_Called_Given_That_BodyGenerator_Is_Null_Sends_Null_Body_To_The_SmtpClientFacade()
        {
            //Arrange
            SmtpClient smtpClient = new SmtpClient();
            MailboxAddress fromAddress = new MailboxAddress("recipient@example.com");
            var recipientProviderMock = new Mock<IRecipientProvider>();
            var recipients = new List<string>() { "recipient1@example.com" };
            recipientProviderMock.Setup(x => x.GetRecipientsAsync(It.IsAny<LogModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(recipients);
            var mailSubjectGeneratorMock = new Mock<ILogSerializer>();
            mailSubjectGeneratorMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns("mailSubject");
            var mailAttachmentGeneratorMock = new Mock<ILogSerializer>();
            mailAttachmentGeneratorMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns("mailAtachment");
            EmailDestination emailDestination = new EmailDestination(smtpClient, fromAddress, recipientProviderMock.Object, mailSubjectGeneratorMock.Object, null, mailAttachmentGeneratorMock.Object);
            var smtpClientFacadeMock = new Mock<ISmtpClientFacade>();
            emailDestination.ResetSmtpClientFacade(smtpClientFacadeMock.Object);
            CancellationToken cancellationToken = new CancellationToken();

            //Act
            await emailDestination.SendAsync(new LogModel[] { new LogModel() }, cancellationToken);

            //Assert
            smtpClientFacadeMock.Verify(x => x.SendEmailAsync(smtpClient, fromAddress, recipients, "mailSubject", null, "mailAtachment", cancellationToken), Times.Once);
        }
        [Test]
        public async Task SendAsync_When_Called_Given_That_AttachmentGenerator_Is_Null_Sends_Null_Attachment_To_The_SmtpClientFacade()
        {
            //Arrange
            SmtpClient smtpClient = new SmtpClient();
            MailboxAddress fromAddress = new MailboxAddress("recipient@example.com");
            var recipientProviderMock = new Mock<IRecipientProvider>();
            var recipients = new List<string>() { "recipient1@example.com" };
            recipientProviderMock.Setup(x => x.GetRecipientsAsync(It.IsAny<LogModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(recipients);
            var mailSubjectGeneratorMock = new Mock<ILogSerializer>();
            mailSubjectGeneratorMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns("mailSubject");
            var mailBodyGeneratorMock = new Mock<ILogSerializer>();
            mailBodyGeneratorMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns("mailBody");
            EmailDestination emailDestination = new EmailDestination(smtpClient, fromAddress, recipientProviderMock.Object, mailSubjectGeneratorMock.Object, mailBodyGeneratorMock.Object, null);
            var smtpClientFacadeMock = new Mock<ISmtpClientFacade>();
            emailDestination.ResetSmtpClientFacade(smtpClientFacadeMock.Object);
            CancellationToken cancellationToken = new CancellationToken();

            //Act
            await emailDestination.SendAsync(new LogModel[] { new LogModel() }, cancellationToken);

            //Assert
            smtpClientFacadeMock.Verify(x => x.SendEmailAsync(smtpClient, fromAddress, recipients, "mailSubject", "mailBody", null, cancellationToken), Times.Once);
        }

        #endregion
    }
}
