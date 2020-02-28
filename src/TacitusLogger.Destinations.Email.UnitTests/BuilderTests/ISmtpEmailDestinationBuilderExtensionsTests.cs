using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TacitusLogger.Builders;
using TacitusLogger.Serializers;

namespace TacitusLogger.Destinations.Email.UnitTests
{
    [TestFixture]
    public class ISmtpEmailDestinationBuilderExtensionsTests
    {
        #region Tests for WithRecipients method overloads

        [Test]
        public void WithRecipients_TakingRecipientsList_When_Called_CreatesNewEmailListRecipientProviderAndPassesRecipientsToItThenReturnsCreatedResult()
        {
            //Arrange 
            var smtpEmailDestinationBuilderMock = new Mock<ISmtpEmailDestinationBuilder>();
            smtpEmailDestinationBuilderMock.Setup(x => x.WithRecipients(It.IsAny<EmailListRecipientProvider>())).Returns(smtpEmailDestinationBuilderMock.Object);
            string[] recipients = new string[] { "recipient@example.com" };

            //Act
            ISmtpEmailDestinationBuilder returnedSmtpEmailDestinationBuilder = ISmtpEmailDestinationBuilderExtensions.WithRecipients(smtpEmailDestinationBuilderMock.Object, recipients);

            //Assert 
            Assert.AreEqual(smtpEmailDestinationBuilderMock.Object, returnedSmtpEmailDestinationBuilder);
            smtpEmailDestinationBuilderMock.Verify(x => x.WithRecipients(It.Is<EmailListRecipientProvider>(p => p.Recipients == recipients)), Times.Once);
        }

        [Test]
        public void WithRecipients_TakingFactoryMethod_When_Called_CreatesNewFactoryMethodRecipientProviderAndPassesFactoryMethodToItThenReturnsCreatedResult()
        {
            //Arrange 
            var smtpEmailDestinationBuilderMock = new Mock<ISmtpEmailDestinationBuilder>();
            smtpEmailDestinationBuilderMock.Setup(x => x.WithRecipients(It.IsAny<FactoryMethodRecipientProvider>())).Returns(smtpEmailDestinationBuilderMock.Object);
            LogModelFunc<ICollection<string>> factoryMethod = d => null;

            //Act
            ISmtpEmailDestinationBuilder returnedSmtpEmailDestinationBuilder = ISmtpEmailDestinationBuilderExtensions.WithRecipients(smtpEmailDestinationBuilderMock.Object, factoryMethod);

            //Assert 
            Assert.AreEqual(smtpEmailDestinationBuilderMock.Object, returnedSmtpEmailDestinationBuilder);
            smtpEmailDestinationBuilderMock.Verify(x => x.WithRecipients(It.Is<FactoryMethodRecipientProvider>(p => p.FactoryMethod == factoryMethod)), Times.Once);
        }

        #endregion

        #region Tests for WithSubject method overloads

        [Test]
        public void WithSubject_TakingTemplateString_When_Called_CreatesNewSimpleTemplateLogSerializerAndPassesTemplateToItThenReturnsCreatedResult()
        {
            //Arrange 
            var smtpEmailDestinationBuilderMock = new Mock<ISmtpEmailDestinationBuilder>();
            smtpEmailDestinationBuilderMock.Setup(x => x.WithSubject(It.IsAny<SimpleTemplateLogSerializer>())).Returns(smtpEmailDestinationBuilderMock.Object);
            string template = "tempalte";

            //Act
            ISmtpEmailDestinationBuilder returnedSmtpEmailDestinationBuilder = ISmtpEmailDestinationBuilderExtensions.WithSubject(smtpEmailDestinationBuilderMock.Object, template);

            //Assert 
            Assert.AreEqual(smtpEmailDestinationBuilderMock.Object, returnedSmtpEmailDestinationBuilder);
            smtpEmailDestinationBuilderMock.Verify(x => x.WithSubject(It.Is<SimpleTemplateLogSerializer>(p => p.Template == template)), Times.Once);
        }


        [Test]
        public void WithSubject_TakingFactoryMethod_When_Called_CreatesNewGeneratorFunctionLogSerializerAndPassesFactoryMethodToItThenReturnsCreatedResult()
        {
            //Arrange 
            var smtpEmailDestinationBuilderMock = new Mock<ISmtpEmailDestinationBuilder>();
            smtpEmailDestinationBuilderMock.Setup(x => x.WithSubject(It.IsAny<GeneratorFunctionLogSerializer>())).Returns(smtpEmailDestinationBuilderMock.Object);
            LogModelFunc<string> factoryMethod = d => null;

            //Act
            ISmtpEmailDestinationBuilder returnedSmtpEmailDestinationBuilder = ISmtpEmailDestinationBuilderExtensions.WithSubject(smtpEmailDestinationBuilderMock.Object, factoryMethod);

            //Assert 
            Assert.AreEqual(smtpEmailDestinationBuilderMock.Object, returnedSmtpEmailDestinationBuilder);
            smtpEmailDestinationBuilderMock.Verify(x => x.WithSubject(It.Is<GeneratorFunctionLogSerializer>(p => p.GeneratorFunction == factoryMethod)), Times.Once);
        }

        #endregion

        #region Tests for WithBody method overloads

        [Test]
        public void WithBody_TakingTemplate_When_Called_CreatesNewExtendedTemplateLogSerializerAndPassesTemplateToItThenReturnsCreatedResult()
        {
            //Arrange 
            var smtpEmailDestinationBuilderMock = new Mock<ISmtpEmailDestinationBuilder>();
            smtpEmailDestinationBuilderMock.Setup(x => x.WithBody(It.IsAny<ExtendedTemplateLogSerializer>())).Returns(smtpEmailDestinationBuilderMock.Object);
            string template = "template";

            //Act
            ISmtpEmailDestinationBuilder returnedSmtpEmailDestinationBuilder = ISmtpEmailDestinationBuilderExtensions.WithBody(smtpEmailDestinationBuilderMock.Object, template);

            //Assert 
            Assert.AreEqual(smtpEmailDestinationBuilderMock.Object, returnedSmtpEmailDestinationBuilder);
            smtpEmailDestinationBuilderMock.Verify(x => x.WithBody(It.Is<ExtendedTemplateLogSerializer>(p => p.Template == template)), Times.Once);
        }

        [Test]
        public void WithBody_TakingFactoryMethod_When_Called_CreatesNewGeneratorFunctionLogSerializerAndPassesFactoryMethodToItThenReturnsCreatedResult()
        {
            //Arrange 
            var smtpEmailDestinationBuilderMock = new Mock<ISmtpEmailDestinationBuilder>();
            smtpEmailDestinationBuilderMock.Setup(x => x.WithBody(It.IsAny<GeneratorFunctionLogSerializer>())).Returns(smtpEmailDestinationBuilderMock.Object);
            LogModelFunc<string> factoryMethod = d => null;

            //Act
            ISmtpEmailDestinationBuilder returnedSmtpEmailDestinationBuilder = ISmtpEmailDestinationBuilderExtensions.WithBody(smtpEmailDestinationBuilderMock.Object, factoryMethod);

            //Assert 
            Assert.AreEqual(smtpEmailDestinationBuilderMock.Object, returnedSmtpEmailDestinationBuilder);
            smtpEmailDestinationBuilderMock.Verify(x => x.WithBody(It.Is<GeneratorFunctionLogSerializer>(p => p.GeneratorFunction == factoryMethod)), Times.Once);
        }

        #endregion
         
        #region Tests for WithAttachment method overloads

        [Test]
        public void WithAttachment_TakingTemplate_When_Called_CreatesNewExtendedTemplateLogSerializerAndPassestemplateToItThenReturnsCreatedResult()
        {
            //Arrange 
            var smtpEmailDestinationBuilderMock = new Mock<ISmtpEmailDestinationBuilder>();
            smtpEmailDestinationBuilderMock.Setup(x => x.WithAttachment(It.IsAny<ExtendedTemplateLogSerializer>())).Returns(smtpEmailDestinationBuilderMock.Object);
            string template = "template";

            //Act
            ISmtpEmailDestinationBuilder returnedSmtpEmailDestinationBuilder = ISmtpEmailDestinationBuilderExtensions.WithAttachment(smtpEmailDestinationBuilderMock.Object, template);

            //Assert 
            Assert.AreEqual(smtpEmailDestinationBuilderMock.Object, returnedSmtpEmailDestinationBuilder);
            smtpEmailDestinationBuilderMock.Verify(x => x.WithAttachment(It.Is<ExtendedTemplateLogSerializer>(p => p.Template == template)), Times.Once);
        }

        [Test]
        public void WithAttachment_TakingFactoryMethod_When_Called_CreatesNewGeneratorFunctionLogSerializerAndPassesFactoryMethodToItThenReturnsCreatedResult()
        {
            //Arrange 
            var smtpEmailDestinationBuilderMock = new Mock<ISmtpEmailDestinationBuilder>();
            smtpEmailDestinationBuilderMock.Setup(x => x.WithAttachment(It.IsAny<GeneratorFunctionLogSerializer>())).Returns(smtpEmailDestinationBuilderMock.Object);
            LogModelFunc<string> factoryMethod = d => null;

            //Act
            ISmtpEmailDestinationBuilder returnedSmtpEmailDestinationBuilder = ISmtpEmailDestinationBuilderExtensions.WithAttachment(smtpEmailDestinationBuilderMock.Object, factoryMethod);

            //Assert 
            Assert.AreEqual(smtpEmailDestinationBuilderMock.Object, returnedSmtpEmailDestinationBuilder);
            smtpEmailDestinationBuilderMock.Verify(x => x.WithAttachment(It.Is<GeneratorFunctionLogSerializer>(p => p.GeneratorFunction == factoryMethod)), Times.Once);
        }

        #endregion
    }
}
