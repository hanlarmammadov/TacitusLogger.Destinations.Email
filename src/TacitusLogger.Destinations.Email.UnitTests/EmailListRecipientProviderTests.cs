using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace TacitusLogger.Destinations.Email.UnitTests
{
    [TestFixture]
    public class EmailListRecipientProviderTests
    {
        #region Ctor tests

        [Test]
        public void Ctor_When_Called_Sets_Recipients_Collection()
        {
            //Act
            EmailListRecipientProvider emailListRecipientProvider = new EmailListRecipientProvider("Recipient1", "Recipient2");

            //Assert
            Assert.AreEqual(2, emailListRecipientProvider.Recipients.Count);
            Assert.IsTrue(emailListRecipientProvider.Recipients.Contains("Recipient1"));
            Assert.IsTrue(emailListRecipientProvider.Recipients.Contains("Recipient2"));
        }

        [Test]
        public void Ctor_When_Called_With_Null_Recipients_List_Throws_ArgumentNullException()
        {
            //Arrange
            string[] recipients = null;

            //Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                //Act
                EmailListRecipientProvider emailListRecipientProvider = new EmailListRecipientProvider(recipients);
            });
        }

        [Test]
        public void Ctor_When_Called_With_Empty_Recipients_List_Throws_ArgumentNullException()
        {
            //Arrange
            string[] recipients = new string[0];

            //Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                //Act
                EmailListRecipientProvider emailListRecipientProvider = new EmailListRecipientProvider(recipients);
            });
        }

        [Test]
        public void Ctor_When_Called_With_Recipients_List_Containing_Null_Strings_Throws_ArgumentException()
        {
            //Arrange
            string[] recipients = new string[3] { "Recipient1", "Recipient2", null };

            //Assert
            Assert.Catch<ArgumentException>(() =>
            {
                //Act
                EmailListRecipientProvider emailListRecipientProvider = new EmailListRecipientProvider(recipients);
            });
        }

        #endregion

        #region Tests for GetRecipients and GetRecipientsAsync methods

        [Test]
        public async Task GetRecipients_When_Called_Returns_Provided_Recipients_List()
        {
            //Arrange
            string[] recipients = new string[2] { "Recipient1", "Recipient2" };
            EmailListRecipientProvider emailListRecipientProvider = new EmailListRecipientProvider(recipients);

            //Act
            var recipientsReturned = emailListRecipientProvider.GetRecipients(new LogModel());
            var recipientsReturnedAsync = await emailListRecipientProvider.GetRecipientsAsync(new LogModel());

            //Assert
            Assert.AreEqual(emailListRecipientProvider.Recipients, recipientsReturned);
            Assert.AreEqual(emailListRecipientProvider.Recipients, recipientsReturnedAsync);
        }

        [Test]
        public async Task GetRecipients_When_Called_With_Null_LogModel_Does_Not_Throw_Exception()
        {
            //Arrange
            string[] recipients = new string[2] { "Recipient1", "Recipient2" };
            EmailListRecipientProvider emailListRecipientProvider = new EmailListRecipientProvider(recipients);

            //Act
            var recipientsReturned = emailListRecipientProvider.GetRecipients(null);
            var recipientsReturnedAsync = await emailListRecipientProvider.GetRecipientsAsync(null);
        }

        #endregion 
    }
}
