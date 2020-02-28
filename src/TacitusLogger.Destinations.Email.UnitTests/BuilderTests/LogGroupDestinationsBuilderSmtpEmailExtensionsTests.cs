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
    public class LogGroupDestinationsBuilderSmtpEmailExtensionsTests
    {
        #region Tests for Email method

        [Test]
        public void Email_When_Called_Creates_New_SmtpEmailDestinationBuilder_Passing_LogGroupDestinationsBuilder_To_It_And_Returns_Created_Result()
        {
            //Arrange 
            var logGroupDestinationsBuilderMock = new Mock<ILogGroupDestinationsBuilder>();
            
            //Act
            ISmtpEmailDestinationBuilder smtpEmailDestinationBuilder = LogGroupDestinationsBuilderSmtpEmailExtensions.Email(logGroupDestinationsBuilderMock.Object);

            //Assert 
            Assert.IsInstanceOf<SmtpEmailDestinationBuilder>(smtpEmailDestinationBuilder);
            Assert.AreEqual(logGroupDestinationsBuilderMock.Object, (smtpEmailDestinationBuilder as SmtpEmailDestinationBuilder).LogGroupDestinationsBuilder);
        }
 
        #endregion 
    }
}
