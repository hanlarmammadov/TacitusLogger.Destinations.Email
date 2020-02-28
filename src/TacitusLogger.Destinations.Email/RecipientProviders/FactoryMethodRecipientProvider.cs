using System;
using System.Collections.Generic;
using System.Text;

namespace TacitusLogger.Destinations.Email
{
    /// <summary>
    /// FactoryMethodRecipientProvider class implements <c>IRecipientProvider</c> interface by means of <c>TacitusLogger.LogModelFunc<ICollection<string>></c> delegate. 
    /// This delegate is expected to take LogModel model and to return list of recipient addresses. Its logic is defined by user. 
    /// </summary>
    public class FactoryMethodRecipientProvider: SynchronousRecipientProviderBase
    {
        private readonly LogModelFunc<ICollection<string>> _factoryMethod;

        /// <summary>
        /// Creates an instance of <c>TacitusLogger.Destinations.Email.RecipientProviders.FactoryMethodRecipientProvider</c> with
        /// specified factory method.
        /// </summary>
        /// <param name="factoryMethod">The factory method.</param>
        public FactoryMethodRecipientProvider(LogModelFunc<ICollection<string>> factoryMethod)
        {
            _factoryMethod = factoryMethod ?? throw new ArgumentNullException("factoryMethod");
        }

        /// <summary>
        /// Gets the factory method provided during the initialization.
        /// </summary>
        public LogModelFunc<ICollection<string>> FactoryMethod => _factoryMethod;

        /// <summary>
        /// Creates the list of email recipient addresses using log data model.
        /// </summary>
        /// <param name="logData">Log data model.</param>
        /// <returns></returns>
        public override ICollection<string> GetRecipients(LogModel logData)
        {
            return _factoryMethod(logData);
        }
    }
}
