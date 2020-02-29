using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic; 
using System.Threading;
using System.Threading.Tasks;
using TacitusLogger.Serializers;

namespace TacitusLogger.Destinations.Email
{
    /// <summary>
    /// Destination that sends log information by email using SMTP.
    /// </summary>
    public class EmailDestination : ILogDestination
    {
        private readonly SmtpClient _smtpClient;
        private readonly MailboxAddress _fromAddress;
        private readonly IRecipientProvider _recipientProvider;
        private readonly ILogSerializer _mailSubjectGenerator;
        private readonly ILogSerializer _mailBodyGenerator;
        private readonly ILogSerializer _mailAttachmentGenerator;
        private ISmtpClientFacade _smtpClientFacade;

        /// <summary>
        /// Creates an instance of <c>TacitusLogger.Destinations.Email.EmailDestination</c>.
        /// </summary>
        /// <param name="smtpClient">SMTP client that will be used to send emails.</param>
        /// <param name="fromAddress">Address that will appear in From line at receivers of log mails.</param>
        /// <param name="recipientProvider">Used to get list of log mail recipients.</param>
        /// <param name="mailSubjectGenerator">Used to generate mail subject text based on log data model.</param>
        /// <param name="mailBodyGenerator">Used to generate mail body text based on log data model.</param>
        public EmailDestination(SmtpClient smtpClient, MailboxAddress fromAddress, IRecipientProvider recipientProvider, ILogSerializer mailSubjectGenerator, ILogSerializer mailBodyGenerator, ILogSerializer mailAttachmentGenerator)
        {
            _smtpClient = smtpClient ?? throw new ArgumentNullException("smtpClient");
            _fromAddress = fromAddress ?? throw new ArgumentNullException("from");
            _recipientProvider = recipientProvider ?? throw new ArgumentNullException("recipientProvider");
            _mailSubjectGenerator = mailSubjectGenerator;
            _mailBodyGenerator = mailBodyGenerator;
            _mailAttachmentGenerator = mailAttachmentGenerator; 
            _smtpClientFacade = new StandardSmtpClientFacade();
        }
        /// <summary>
        /// Creates an instance of <c>TacitusLogger.Destinations.Email.EmailDestination</c> using default mail subject generator and mail body generator.
        /// </summary>
        /// <param name="smtpClient">SMTP client that will be used to send emails.</param>
        /// <param name="fromAddress">Address that will appear in From line at receivers of log mails.</param>
        /// <param name="recipients">List of recipient email addresses.</param>
        public EmailDestination(SmtpClient smtpClient, MailboxAddress fromAddress, params string[] recipients)
            : this(smtpClient, fromAddress, new EmailListRecipientProvider(recipients), new SimpleTemplateLogSerializer(), new ExtendedTemplateLogSerializer(), null)
        {

        } 
        public EmailDestination(SmtpClient smtpClient, MailboxAddress fromAddress, IRecipientProvider recipientProvider)
            : this(smtpClient, fromAddress, recipientProvider, new SimpleTemplateLogSerializer(), new ExtendedTemplateLogSerializer(), null)
        {

        } 
        public EmailDestination(SmtpClient smtpClient, MailboxAddress fromAddress, IRecipientProvider recipientProvider, ILogSerializer mailSubjectGenerator)
            : this(smtpClient, fromAddress, recipientProvider, mailSubjectGenerator, new ExtendedTemplateLogSerializer(), null)
        {

        } 
        public EmailDestination(SmtpClient smtpClient, MailboxAddress fromAddress, IRecipientProvider recipientProvider, ILogSerializer mailSubjectGenerator, ILogSerializer mailBodyGenerator)
            : this(smtpClient, fromAddress, recipientProvider, mailSubjectGenerator, mailBodyGenerator, null)
        {

        }

        /// <summary>
        /// Gets SMTP client that was specified during initialization.
        /// </summary>
        public SmtpClient SmtpClient => _smtpClient;
        /// <summary>
        /// Gets address that will be used as sender address for log mails.
        /// </summary>
        public MailboxAddress From => _fromAddress;
        /// <summary>
        /// Gets recipient provider that was specified during initialization.
        /// </summary>
        public IRecipientProvider RecipientProvider => _recipientProvider;
        /// <summary>
        /// Gets mail subject generator that was specified during initialization.
        /// </summary>
        public ILogSerializer MailSubjectGenerator => _mailSubjectGenerator;
        /// <summary>
        /// Gets mail body generator that was specified during initialization.
        /// </summary>
        public ILogSerializer MailBodyGenerator => _mailBodyGenerator;
        /// <summary>
        /// Gets mail attachment generator that was specified during initialization.
        /// </summary>
        public ILogSerializer MailAttachmentGenerator => _mailAttachmentGenerator;
        /// <summary>
        /// Gets SMTP client facade that was specified during initialization. For test purposes.
        /// </summary>
        internal ISmtpClientFacade SmtpClientFacade => _smtpClientFacade;

        /// <summary>
        /// Writes log data models to the destination.
        /// </summary>
        /// <param name="logs">Log data models array.</param>
        public void Send(LogModel[] logs)
        {
            for (int i = 0; i < logs.Length; i++)
            {
                // Get recipients list.
                ICollection<string> recipients = _recipientProvider.GetRecipients(logs[i]);

                // Generate mail subject.
                string mailSubject = null;
                if (_mailSubjectGenerator != null)
                    mailSubject = _mailSubjectGenerator.Serialize(logs[i]);

                // Generate mail body.
                string mailBody = null;
                if (_mailBodyGenerator != null)
                    mailBody = _mailBodyGenerator.Serialize(logs[i]);

                // Generate mail attachment.
                string mailAttachment = null;
                if (_mailAttachmentGenerator != null)
                    mailAttachment = _mailAttachmentGenerator.Serialize(logs[i]);

                // Send email.
                _smtpClientFacade.SendEmail(_smtpClient, _fromAddress, recipients, mailSubject, mailBody, mailAttachment);
            }
        }
        /// <summary>
        /// Asynchronously writes log data models to the destination.
        /// </summary>
        /// <param name="logs">Log data models array.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SendAsync(LogModel[] logs, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (cancellationToken.IsCancellationRequested)
                await Task.FromCanceled(cancellationToken);

            for (int i = 0; i < logs.Length; i++)
            {
                // Get recipients list.
                ICollection<string> recipients = await _recipientProvider.GetRecipientsAsync(logs[i], cancellationToken);

                // Generate mail subject.
                string mailSubject = null;
                if (_mailSubjectGenerator != null)
                    mailSubject = _mailSubjectGenerator.Serialize(logs[i]);

                // Generate mail body.
                string mailBody = null;
                if (_mailBodyGenerator != null)
                    mailBody = _mailBodyGenerator.Serialize(logs[i]);

                // Generate mail attachment.
                string mailAttachment = null;
                if (_mailAttachmentGenerator != null)
                    mailAttachment = _mailAttachmentGenerator.Serialize(logs[i]);

                // Send the email.
                await _smtpClientFacade.SendEmailAsync(_smtpClient, _fromAddress, recipients, mailSubject, mailBody, mailAttachment, cancellationToken);
            }
        }
        public void Dispose()
        {

        }
        /// <summary>
        /// Resets the SMTP client facade that was set during initialization. For testing purposes.
        /// </summary>
        /// <param name="smtpClientFacade">New SMTP client facade</param>
        internal void ResetSmtpClientFacade(ISmtpClientFacade smtpClientFacade)
        {
            _smtpClientFacade = smtpClientFacade;
        }
    }
}
