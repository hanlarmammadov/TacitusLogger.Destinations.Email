using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TacitusLogger.Destinations.Email.UnitTests
{
    [TestFixture]
    public class FactoryMethodRecipientProviderTests
    {
        #region Ctor tests

        [Test]
        public void Ctor_When_Called_Sets_The_Factory_Method_Property()
        {
            //Arrange
            LogModelFunc<ICollection<string>> factoryMethod = (d) => null;

            //Act
            FactoryMethodRecipientProvider factoryMethodRecipientProvider = new FactoryMethodRecipientProvider(factoryMethod);

            //Assert
            Assert.AreEqual(factoryMethod, factoryMethodRecipientProvider.FactoryMethod);
        }
        [Test]
        public void Ctor_When_Called_With_Null_Factory_Method_Throws_ArgumentNullException()
        {
            //Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                //Act
                FactoryMethodRecipientProvider factoryMethodRecipientProvider = new FactoryMethodRecipientProvider(null);
            });
        }

        #endregion

        #region Tests for GetRecipients and GetRecipientsAsync methods

        [Test]
        public async Task GetRecipients_When_Called_Returns_Recipient_List_In_Accordance_To_Factory_Method_Logic()
        {
            //Arrange
            List<string> recipients = new List<string>() { "Recipient1", "Recipient2" };
            LogModelFunc<ICollection<string>> factoryMethod = (d) => recipients;
            FactoryMethodRecipientProvider factoryMethodRecipientProvider = new FactoryMethodRecipientProvider(factoryMethod);

            //Act
            var recipientsReturned = factoryMethodRecipientProvider.GetRecipients(new LogModel());
            var recipientsReturnedAsync = await factoryMethodRecipientProvider.GetRecipientsAsync(new LogModel());

            //Assert
            Assert.AreEqual(recipients, recipientsReturned);
            Assert.AreEqual(recipients, recipientsReturnedAsync);
        }
        [Test]
        public async Task GetRecipients_When_Called_With_Null_LogModel_Does_Not_Throw_Exception()
        {
            //Arrange
            LogModelFunc<ICollection<string>> factoryMethod = (d) => new List<string>() { };
            FactoryMethodRecipientProvider factoryMethodRecipientProvider = new FactoryMethodRecipientProvider(factoryMethod);

            //Act
            var recipientsReturned = factoryMethodRecipientProvider.GetRecipients(null);
            var recipientsReturnedAsync = await factoryMethodRecipientProvider.GetRecipientsAsync(null);
        }

        #endregion 
    }
}
