using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TacitusLogger.Destinations.Email
{
    /// <summary>
    /// Convenient to be inherited from if the recipients selection operation represents a quick synchronous process
    /// which does not assume any specific overriding of the async counterpart of GetRecipients method.
    /// </summary>
    public abstract class SynchronousRecipientProviderBase : IRecipientProvider
    {
        /// <summary>
        /// Should be overridden in subclasses.
        /// </summary>
        /// <param name="logData"></param>
        /// <returns></returns>
        public abstract ICollection<string> GetRecipients(LogModel logData);
        /// <summary>
        /// Asynchronous counterpart of GetRecipients method.
        /// </summary>
        /// <param name="logData">Log data model.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents the represents collection.</returns>
        public Task<ICollection<string>> GetRecipientsAsync(LogModel logData, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (cancellationToken.IsCancellationRequested)
                return Task.FromCanceled<ICollection<string>>(cancellationToken);

            return Task.FromResult<ICollection<string>>(GetRecipients(logData));
        }
    }
}
