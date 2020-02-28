using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TacitusLogger.Destinations.Email
{
    public interface IRecipientProvider
    {
        ICollection<string> GetRecipients(LogModel logData);
        Task<ICollection<string>> GetRecipientsAsync(LogModel logData, CancellationToken cancellationToken = default(CancellationToken));
    }
}
