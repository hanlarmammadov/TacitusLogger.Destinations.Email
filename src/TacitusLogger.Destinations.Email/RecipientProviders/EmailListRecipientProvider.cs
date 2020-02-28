using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TacitusLogger.Destinations.Email
{
    public class EmailListRecipientProvider : SynchronousRecipientProviderBase
    {
        private readonly ICollection<string> _recipients;

        public EmailListRecipientProvider(params string[] recipients)
        {
            if (recipients == null || recipients.Length == 0)
                throw new ArgumentNullException("recipients");
            if (recipients.Contains(null))
                throw new ArgumentException("Recipients list contains null values");

            _recipients = recipients; 
        }

        internal ICollection<string> Recipients => _recipients;
         
        public override ICollection<string> GetRecipients(LogModel logData)
        {
            return _recipients;
        }
    }
}
