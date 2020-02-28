using MailKit.Net.Smtp;
using MimeKit;
using Moq;
using NUnit.Framework;
using System;
using TacitusLogger.Builders;
using TacitusLogger.Serializers;

namespace TacitusLogger.Destinations.Email.UnitTests
{
    [TestFixture]
    public class SmtpEmailDestinationBuilderTests
    {
        #region Ctor tests

        [Test]
        public void Ctor_When_Called_Sets_LogGroupDestinationsBuilder()
        {
            //Arrange 
            var logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;


            //Act
            SmtpEmailDestinationBuilder smtpEmailDestinationBuilder = new SmtpEmailDestinationBuilder(logGroupDestinationsBuilder);

            //Assert
            Assert.AreEqual(logGroupDestinationsBuilder, smtpEmailDestinationBuilder.LogGroupDestinationsBuilder);
        }
        [Test]
        public void Ctor_When_Called_All_Email_Destination_Dependencies_Are_Set_To_Null()
        {
            //Arrange 
            var logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;

            //Act
            SmtpEmailDestinationBuilder smtpEmailDestinationBuilder = new SmtpEmailDestinationBuilder(logGroupDestinationsBuilder);

            //Assert
            Assert.IsNull(smtpEmailDestinationBuilder.SmtpClient);
            Assert.IsNull(smtpEmailDestinationBuilder.FromAddress);
            Assert.IsNull(smtpEmailDestinationBuilder.RecipientProvider);
            Assert.IsNull(smtpEmailDestinationBuilder.MailSubjectGenerator);
            Assert.IsNull(smtpEmailDestinationBuilder.MailBodyGenerator);
            Assert.IsNull(smtpEmailDestinationBuilder.MailAttachmentGenerator);
        }

        #endregion

        #region Tests for WithXXX method

        [Test]
        public void WithSmtpClient_Taking_SmtpClient_And_FromAddress_When_Called_Sets_Builders_SmtpClient_And_FromAddress()
        {
            //Arrange 
            var logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;
            SmtpEmailDestinationBuilder smtpEmailDestinationBuilder = new SmtpEmailDestinationBuilder(logGroupDestinationsBuilder);
            SmtpClient smtpClient = new SmtpClient();
            MailboxAddress fromAddress = new MailboxAddress("recipient@example.com");

            //Act
            smtpEmailDestinationBuilder.WithSmtpClient(smtpClient, fromAddress);

            //Assert
            Assert.AreEqual(smtpClient, smtpEmailDestinationBuilder.SmtpClient);
            Assert.AreEqual(fromAddress, smtpEmailDestinationBuilder.FromAddress);
        }
        [Test]
        public void WithSmtpClient_Taking_SmtpClient_And_FromAddress_When_Called_Given_That_Already_Set_Throws_InvalidOperationException()
        {
            //Arrange 
            var logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;
            SmtpEmailDestinationBuilder smtpEmailDestinationBuilder = new SmtpEmailDestinationBuilder(logGroupDestinationsBuilder);
            smtpEmailDestinationBuilder.WithSmtpClient(new SmtpClient(), new MailboxAddress("recipient1@example.com"));

            //Assert
            Assert.Catch<InvalidOperationException>(() =>
            {
                //Act
                smtpEmailDestinationBuilder.WithSmtpClient(new SmtpClient(), new MailboxAddress("recipient2@example.com"));
            });
        }
        [Test]
        public void WithSmtpClient_Taking_SmtpClient_And_FromAddress_When_Called_With_Null_SmtpClient_Throws_ArgumentNullException()
        {
            //Arrange 
            var logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;
            SmtpEmailDestinationBuilder smtpEmailDestinationBuilder = new SmtpEmailDestinationBuilder(logGroupDestinationsBuilder);
            var fromAddress = new MailboxAddress("recipient1@example.com");

            //Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                //Act
                smtpEmailDestinationBuilder.WithSmtpClient(null, fromAddress);
            });
        }
        [Test]
        public void WithSmtpClient_Taking_SmtpClient_And_FromAddress_When_Called_With_Null_FromAddress_Throws_ArgumentNullException()
        {
            //Arrange 
            var logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;
            SmtpEmailDestinationBuilder smtpEmailDestinationBuilder = new SmtpEmailDestinationBuilder(logGroupDestinationsBuilder);
            var smtpClient = new SmtpClient();

            //Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                //Act
                smtpEmailDestinationBuilder.WithSmtpClient(smtpClient, null as MailboxAddress);
            });
        }
        [Test]
        public void WithSmtpClient_Taking_SmtpClient_And_FromAddress_String_When_Called_Sets_Builders_SmtpClient_And_FromAddress()
        {
            //Arrange 
            var logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;
            SmtpEmailDestinationBuilder smtpEmailDestinationBuilder = new SmtpEmailDestinationBuilder(logGroupDestinationsBuilder);
            SmtpClient smtpClient = new SmtpClient();
            string fromAddressString = "recipient@example.com";

            //Act
            smtpEmailDestinationBuilder.WithSmtpClient(smtpClient, fromAddressString);

            //Assert
            Assert.AreEqual(smtpClient, smtpEmailDestinationBuilder.SmtpClient);
            Assert.AreEqual(fromAddressString, smtpEmailDestinationBuilder.FromAddress.Address);
            Assert.IsNull(smtpEmailDestinationBuilder.FromAddress.Name); 
        }
        [Test]
        public void WithSmtpClient_Taking_SmtpClient_And_FromAddress_String_When_Called_Given_That_Already_Set_Throws_InvalidOperationException()
        {
            //Arrange 
            var logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;
            SmtpEmailDestinationBuilder smtpEmailDestinationBuilder = new SmtpEmailDestinationBuilder(logGroupDestinationsBuilder);
            smtpEmailDestinationBuilder.WithSmtpClient(new SmtpClient(), new MailboxAddress("recipient1@example.com"));

            //Assert
            Assert.Catch<InvalidOperationException>(() =>
            {
                //Act
                smtpEmailDestinationBuilder.WithSmtpClient(new SmtpClient(), "recipient2@example.com");
            });
        }
        [Test]
        public void WithSmtpClient_Taking_SmtpClient_And_FromAddress_String_When_Called_With_Null_SmtpClient_Throws_ArgumentNullException()
        {
            //Arrange 
            var logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;
            SmtpEmailDestinationBuilder smtpEmailDestinationBuilder = new SmtpEmailDestinationBuilder(logGroupDestinationsBuilder);
            var fromAddressString = "recipient1@example.com";

            //Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                //Act
                smtpEmailDestinationBuilder.WithSmtpClient(null, fromAddressString);
            });
        }
        [Test]
        public void WithSmtpClient_Taking_SmtpClient_And_FromAddress_String_When_Called_With_Null_FromAddress_String_Throws_ArgumentNullException()
        {
            //Arrange 
            var logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;
            SmtpEmailDestinationBuilder smtpEmailDestinationBuilder = new SmtpEmailDestinationBuilder(logGroupDestinationsBuilder);
            var smtpClient = new SmtpClient();

            //Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                //Act
                smtpEmailDestinationBuilder.WithSmtpClient(smtpClient, null as string);
            });
        }
        [Test]
        public void WithRecipients_Taking_Custom_RecipientProvider_When_Called_Sets_Builders_RecipientProvider()
        {
            //Arrange 
            var logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;
            SmtpEmailDestinationBuilder smtpEmailDestinationBuilder = new SmtpEmailDestinationBuilder(logGroupDestinationsBuilder);
            var recipientProvider = new Mock<IRecipientProvider>().Object;

            //Act
            smtpEmailDestinationBuilder.WithRecipients(recipientProvider);

            //Assert
            Assert.AreEqual(recipientProvider, smtpEmailDestinationBuilder.RecipientProvider);
        }
        [Test]
        public void WithRecipients_Taking_Custom_RecipientProvider_When_Called_Given_That_Already_Set_Throws_InvalidOperationException()
        {
            //Arrange 
            var logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;
            SmtpEmailDestinationBuilder smtpEmailDestinationBuilder = new SmtpEmailDestinationBuilder(logGroupDestinationsBuilder);
            smtpEmailDestinationBuilder.WithRecipients(new Mock<IRecipientProvider>().Object);

            //Assert
            Assert.Catch<InvalidOperationException>(() =>
            {
                //Act
                smtpEmailDestinationBuilder.WithRecipients(new Mock<IRecipientProvider>().Object);
            });
        }
        [Test]
        public void WithRecipients_Taking_Custom_RecipientProvider_When_Called_With_Null_RecipientProvider_Throws_ArgumentNullException()
        {
            //Arrange 
            var logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;
            SmtpEmailDestinationBuilder smtpEmailDestinationBuilder = new SmtpEmailDestinationBuilder(logGroupDestinationsBuilder);

            //Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                //Act
                smtpEmailDestinationBuilder.WithRecipients(null);
            });
        }
        [Test]
        public void WithSubject_Taking_Custom_SubjectGenerator_When_Called_Sets_Builders_Mail_SubjectGenerator()
        {
            //Arrange 
            var logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;
            SmtpEmailDestinationBuilder smtpEmailDestinationBuilder = new SmtpEmailDestinationBuilder(logGroupDestinationsBuilder);
            var mailSubjectGenerator = new Mock<ILogSerializer>().Object;

            //Act
            smtpEmailDestinationBuilder.WithSubject(mailSubjectGenerator);

            //Assert
            Assert.AreEqual(mailSubjectGenerator, smtpEmailDestinationBuilder.MailSubjectGenerator);
            Assert.IsNull(smtpEmailDestinationBuilder.MailBodyGenerator);
            Assert.IsNull(smtpEmailDestinationBuilder.MailAttachmentGenerator);
        }
        [Test]
        public void WithSubject_Taking_Custom_SubjectGenerator_When_Called_Given_That_Already_Set_Throws_InvalidOperationException()
        {
            //Arrange 
            var logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;
            SmtpEmailDestinationBuilder smtpEmailDestinationBuilder = new SmtpEmailDestinationBuilder(logGroupDestinationsBuilder);
            smtpEmailDestinationBuilder.WithSubject(new Mock<ILogSerializer>().Object);

            //Assert
            Assert.Catch<InvalidOperationException>(() =>
            {
                //Act
                smtpEmailDestinationBuilder.WithSubject(new Mock<ILogSerializer>().Object);
            });
        }
        [Test]
        public void WithSubject_Taking_Custom_SubjectGenerator_When_Called_With_Null_Mail_SubjectGenerator_Throws_ArgumentNullException()
        {
            //Arrange 
            var logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;
            SmtpEmailDestinationBuilder smtpEmailDestinationBuilder = new SmtpEmailDestinationBuilder(logGroupDestinationsBuilder);

            //Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                //Act
                smtpEmailDestinationBuilder.WithSubject(null);
            });
        }
        [Test]
        public void WithBody_Taking_Custom_BodyGenerator_When_Called_Sets_Builders_Mail_BodyGenerator()
        {
            //Arrange 
            var logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;
            SmtpEmailDestinationBuilder smtpEmailDestinationBuilder = new SmtpEmailDestinationBuilder(logGroupDestinationsBuilder);
            var mailBodyGenerator = new Mock<ILogSerializer>().Object;

            //Act
            smtpEmailDestinationBuilder.WithBody(mailBodyGenerator);

            //Assert
            Assert.AreEqual(mailBodyGenerator, smtpEmailDestinationBuilder.MailBodyGenerator);
            Assert.IsNull(smtpEmailDestinationBuilder.MailSubjectGenerator);
            Assert.IsNull(smtpEmailDestinationBuilder.MailAttachmentGenerator);
        }
        [Test]
        public void WithBody_Taking_Custom_BodyGenerator_When_Called_Given_That_Already_Set_Throws_InvalidOperationException()
        {
            //Arrange 
            var logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;
            SmtpEmailDestinationBuilder smtpEmailDestinationBuilder = new SmtpEmailDestinationBuilder(logGroupDestinationsBuilder);
            smtpEmailDestinationBuilder.WithBody(new Mock<ILogSerializer>().Object);

            //Assert
            Assert.Catch<InvalidOperationException>(() =>
            {
                //Act
                smtpEmailDestinationBuilder.WithBody(new Mock<ILogSerializer>().Object);
            });
        }
        [Test]
        public void WithBody_Taking_Custom_BodyGenerator_When_Called_With_Null_Mail_BodyGenerator_Throws_ArgumentNullException()
        {
            //Arrange 
            var logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;
            SmtpEmailDestinationBuilder smtpEmailDestinationBuilder = new SmtpEmailDestinationBuilder(logGroupDestinationsBuilder);

            //Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                //Act
                smtpEmailDestinationBuilder.WithBody(null);
            });
        }
        [Test]
        public void WithAttachment_Taking_Custom_AttachmentGenerator_When_Called_Sets_Builders_Mail_AttachmentGenerator()
        {
            //Arrange 
            var logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;
            SmtpEmailDestinationBuilder smtpEmailDestinationBuilder = new SmtpEmailDestinationBuilder(logGroupDestinationsBuilder);
            var mailAttachmentGenerator = new Mock<ILogSerializer>().Object;

            //Act
            smtpEmailDestinationBuilder.WithAttachment(mailAttachmentGenerator);

            //Assert
            Assert.AreEqual(mailAttachmentGenerator, smtpEmailDestinationBuilder.MailAttachmentGenerator);
            Assert.IsNull(smtpEmailDestinationBuilder.MailSubjectGenerator);
            Assert.IsNull(smtpEmailDestinationBuilder.MailBodyGenerator);
        }
        [Test]
        public void WithAttachment_Taking_Custom_AttachmentGenerator_When_Called_Given_That_Already_Set_Throws_InvalidOperationException()
        {
            //Arrange 
            var logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;
            SmtpEmailDestinationBuilder smtpEmailDestinationBuilder = new SmtpEmailDestinationBuilder(logGroupDestinationsBuilder);
            smtpEmailDestinationBuilder.WithAttachment(new Mock<ILogSerializer>().Object);

            //Assert
            Assert.Catch<InvalidOperationException>(() =>
            {
                //Act
                smtpEmailDestinationBuilder.WithAttachment(new Mock<ILogSerializer>().Object);
            });
        }
        [Test]
        public void WithAttachment_Taking_Custom_AttachmentGenerator_When_CalledWithNullMailAttachmentGenerator_Throws_ArgumentNullException()
        {
            //Arrange 
            var logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;
            SmtpEmailDestinationBuilder smtpEmailDestinationBuilder = new SmtpEmailDestinationBuilder(logGroupDestinationsBuilder);

            //Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                //Act
                smtpEmailDestinationBuilder.WithAttachment(null);
            });
        }

        #endregion

        #region Tests for Add method

        [Test]
        public void Add_When_Called_Given_That_All_Dependencies_Was_Set_Explicitly_Creates_EmailDestination_And_Passes_It_To_Log_Group_DestinationBuilders_AddDestination_Method()
        {
            //Arrange 
            var logGroupDestinationsBuilderMock = new Mock<ILogGroupDestinationsBuilder>();
            SmtpEmailDestinationBuilder smtpEmailDestinationBuilder = new SmtpEmailDestinationBuilder(logGroupDestinationsBuilderMock.Object);

            SmtpClient smtpClient = new SmtpClient(); 
            MailboxAddress fromAddress = new MailboxAddress("recipient@example.com");
            smtpEmailDestinationBuilder.WithSmtpClient(smtpClient, fromAddress);
            var recipientProvider = new Mock<IRecipientProvider>().Object;
            smtpEmailDestinationBuilder.WithRecipients(recipientProvider);
            var mailSubjectGenerator = new Mock<ILogSerializer>().Object;
            smtpEmailDestinationBuilder.WithSubject(mailSubjectGenerator);
            var mailBodyGenerator = new Mock<ILogSerializer>().Object;
            smtpEmailDestinationBuilder.WithBody(mailBodyGenerator);
            var mailAttachmentGenerator = new Mock<ILogSerializer>().Object;
            smtpEmailDestinationBuilder.WithAttachment(mailAttachmentGenerator);

            //Act
            smtpEmailDestinationBuilder.Add();

            //Assert
            logGroupDestinationsBuilderMock.Verify(x => x.CustomDestination(It.Is<EmailDestination>(d => d.SmtpClient == smtpClient &&
                                                                                                         d.From == fromAddress &&
                                                                                                         d.RecipientProvider == recipientProvider &&
                                                                                                         d.MailSubjectGenerator == mailSubjectGenerator &&
                                                                                                         d.MailBodyGenerator == mailBodyGenerator &&
                                                                                                         d.MailAttachmentGenerator == mailAttachmentGenerator)), Times.Once);
        }
        [Test]
        public void Add_When_Called_Returns_Result_From_CustomDestination_Method()
        {
            //Arrange 
            var logGroupDestinationsBuilderMock = new Mock<ILogGroupDestinationsBuilder>();
            logGroupDestinationsBuilderMock.Setup(x => x.CustomDestination(It.IsAny<ILogDestination>())).Returns(logGroupDestinationsBuilderMock.Object);
            SmtpEmailDestinationBuilder smtpEmailDestinationBuilder = new SmtpEmailDestinationBuilder(logGroupDestinationsBuilderMock.Object);

            SmtpClient smtpClient = new SmtpClient();
            MailboxAddress fromAddress = new MailboxAddress("recipient@example.com");
            smtpEmailDestinationBuilder.WithSmtpClient(smtpClient, fromAddress);
            var recipientProvider = new Mock<IRecipientProvider>().Object;
            smtpEmailDestinationBuilder.WithRecipients(recipientProvider);
            var mailSubjectGenerator = new Mock<ILogSerializer>().Object;
            smtpEmailDestinationBuilder.WithSubject(mailSubjectGenerator);
            var mailBodyGenerator = new Mock<ILogSerializer>().Object;
            smtpEmailDestinationBuilder.WithBody(mailBodyGenerator);
            var mailAttachmentGenerator = new Mock<ILogSerializer>().Object;
            smtpEmailDestinationBuilder.WithAttachment(mailAttachmentGenerator);

            //Act
            var returnedLogGroupDestinationsBuilder = smtpEmailDestinationBuilder.Add();

            //Assert 
            Assert.AreEqual(logGroupDestinationsBuilderMock.Object, returnedLogGroupDestinationsBuilder);
        }
        [Test]
        public void Add_When_Called_Given_That_SmtpClient_And_FromAddress_Was_Not_Specified_Throws_InvalidOperationException()
        {
            //Arrange 
            var logGroupDestinationsBuilderMock = new Mock<ILogGroupDestinationsBuilder>();
            SmtpEmailDestinationBuilder smtpEmailDestinationBuilder = new SmtpEmailDestinationBuilder(logGroupDestinationsBuilderMock.Object);
 
            var recipientProvider = new Mock<IRecipientProvider>().Object;
            smtpEmailDestinationBuilder.WithRecipients(recipientProvider);
            var mailSubjectGenerator = new Mock<ILogSerializer>().Object;
            smtpEmailDestinationBuilder.WithSubject(mailSubjectGenerator);
            var mailBodyGenerator = new Mock<ILogSerializer>().Object;
            smtpEmailDestinationBuilder.WithBody(mailBodyGenerator);
            var mailAttachmentGenerator = new Mock<ILogSerializer>().Object;
            smtpEmailDestinationBuilder.WithAttachment(mailAttachmentGenerator);

            Assert.Catch<InvalidOperationException>(() =>
            {
                //Act
                smtpEmailDestinationBuilder.Add();
            });
        }
        [Test]
        public void Add_When_Called_Given_That_RecipientProvider_Was_Not_Specified_Throws_InvalidOperationException()
        {
            //Arrange 
            var logGroupDestinationsBuilderMock = new Mock<ILogGroupDestinationsBuilder>();
            SmtpEmailDestinationBuilder smtpEmailDestinationBuilder = new SmtpEmailDestinationBuilder(logGroupDestinationsBuilderMock.Object);

            SmtpClient smtpClient = new SmtpClient();
            MailboxAddress fromAddress = new MailboxAddress("recipient@example.com");
            smtpEmailDestinationBuilder.WithSmtpClient(smtpClient, fromAddress);
            var mailSubjectGenerator = new Mock<ILogSerializer>().Object;
            smtpEmailDestinationBuilder.WithSubject(mailSubjectGenerator);
            var mailBodyGenerator = new Mock<ILogSerializer>().Object;
            smtpEmailDestinationBuilder.WithBody(mailBodyGenerator);
            var mailAttachmentGenerator = new Mock<ILogSerializer>().Object;
            smtpEmailDestinationBuilder.WithAttachment(mailAttachmentGenerator);

            Assert.Catch<InvalidOperationException>(() =>
            {
                //Act
                smtpEmailDestinationBuilder.Add();
            });
        }
        [Test]
        public void Add_When_Called_Given_That_Mail_SubjectGenerator_Was_Not_Specified_Sets_Default_Mail_SubjectGenerator()
        {
            //Arrange 
            var logGroupDestinationsBuilderMock = new Mock<ILogGroupDestinationsBuilder>();
            SmtpEmailDestinationBuilder smtpEmailDestinationBuilder = new SmtpEmailDestinationBuilder(logGroupDestinationsBuilderMock.Object);

            SmtpClient smtpClient = new SmtpClient();
            MailboxAddress fromAddress = new MailboxAddress("recipient@example.com");
            smtpEmailDestinationBuilder.WithSmtpClient(smtpClient, fromAddress);
            var recipientProvider = new Mock<IRecipientProvider>().Object;
            smtpEmailDestinationBuilder.WithRecipients(recipientProvider);
            var mailBodyGenerator = new Mock<ILogSerializer>().Object;
            smtpEmailDestinationBuilder.WithBody(mailBodyGenerator);
            var mailAttachmentGenerator = new Mock<ILogSerializer>().Object;
            smtpEmailDestinationBuilder.WithAttachment(mailAttachmentGenerator);


            //Act
            smtpEmailDestinationBuilder.Add();

            //Assert
            logGroupDestinationsBuilderMock.Verify(x => x.CustomDestination(It.Is<EmailDestination>(d => d.SmtpClient == smtpClient &&
                                                                                              d.From == fromAddress &&
                                                                                              d.RecipientProvider == recipientProvider &&
                                                                                              d.MailSubjectGenerator is SimpleTemplateLogSerializer &&
                                                                                              d.MailBodyGenerator == mailBodyGenerator &&
                                                                                              d.MailAttachmentGenerator == mailAttachmentGenerator)), Times.Once);
        }
        [Test]
        public void Add_When_Called_Given_That_Mail_Body_Generator_Was_Not_Specified_Sets_Default_Mail_BodyGenerator()
        {
            //Arrange 
            var logGroupDestinationsBuilderMock = new Mock<ILogGroupDestinationsBuilder>();
            SmtpEmailDestinationBuilder smtpEmailDestinationBuilder = new SmtpEmailDestinationBuilder(logGroupDestinationsBuilderMock.Object);

            SmtpClient smtpClient = new SmtpClient();
            MailboxAddress fromAddress = new MailboxAddress("recipient@example.com");
            smtpEmailDestinationBuilder.WithSmtpClient(smtpClient, fromAddress);
            var recipientProvider = new Mock<IRecipientProvider>().Object;
            smtpEmailDestinationBuilder.WithRecipients(recipientProvider);
            var mailSubjectGenerator = new Mock<ILogSerializer>().Object;
            smtpEmailDestinationBuilder.WithSubject(mailSubjectGenerator);
            var mailAttachmentGenerator = new Mock<ILogSerializer>().Object;
            smtpEmailDestinationBuilder.WithAttachment(mailAttachmentGenerator);


            //Act
            smtpEmailDestinationBuilder.Add();

            //Assert
            logGroupDestinationsBuilderMock.Verify(x => x.CustomDestination(It.Is<EmailDestination>(d => d.SmtpClient == smtpClient &&
                                                                                              d.From == fromAddress &&
                                                                                              d.RecipientProvider == recipientProvider &&
                                                                                              d.MailSubjectGenerator == mailSubjectGenerator &&
                                                                                              d.MailBodyGenerator is ExtendedTemplateLogSerializer &&
                                                                                              d.MailAttachmentGenerator == mailAttachmentGenerator)), Times.Once);
        }
        [Test]
        public void Add_When_Called_Given_That_Mail_AttachmentGenerator_Was_Not_Specified_Sets_AttachmentGenerator_To_Null()
        {
            //Arrange 
            var logGroupDestinationsBuilderMock = new Mock<ILogGroupDestinationsBuilder>();
            SmtpEmailDestinationBuilder smtpEmailDestinationBuilder = new SmtpEmailDestinationBuilder(logGroupDestinationsBuilderMock.Object);

            SmtpClient smtpClient = new SmtpClient();
            MailboxAddress fromAddress = new MailboxAddress("recipient@example.com");
            smtpEmailDestinationBuilder.WithSmtpClient(smtpClient, fromAddress);
            var recipientProvider = new Mock<IRecipientProvider>().Object;
            smtpEmailDestinationBuilder.WithRecipients(recipientProvider);
            var mailSubjectGenerator = new Mock<ILogSerializer>().Object;
            smtpEmailDestinationBuilder.WithSubject(mailSubjectGenerator);
            var mailBodyGenerator = new Mock<ILogSerializer>().Object;
            smtpEmailDestinationBuilder.WithBody(mailBodyGenerator);

            //Act
            smtpEmailDestinationBuilder.Add();

            //Assert
            logGroupDestinationsBuilderMock.Verify(x => x.CustomDestination(It.Is<EmailDestination>(d => d.SmtpClient == smtpClient &&
                                                                                              d.From == fromAddress &&
                                                                                              d.RecipientProvider == recipientProvider &&
                                                                                              d.MailSubjectGenerator == mailSubjectGenerator &&
                                                                                              d.MailBodyGenerator == mailBodyGenerator &&
                                                                                              d.MailAttachmentGenerator == null)), Times.Once);
        }

        #endregion
    }
}
