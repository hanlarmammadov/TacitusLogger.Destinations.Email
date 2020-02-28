using MailKit.Net.Smtp;
using MimeKit;
using System; 
using TacitusLogger.Builders;
using TacitusLogger.Serializers;

namespace TacitusLogger.Destinations.Email
{
    /// <summary>
    /// Builds and adds an instance of <c>TacitusLogger.Destinations.Email.SmtpEmailDestination</c> class to the <c>TacitusLogger.Builders.ILogGroupDestinationsBuilder</c>
    /// </summary>
    public class SmtpEmailDestinationBuilder : ISmtpEmailDestinationBuilder
    {
        private readonly ILogGroupDestinationsBuilder _logGroupDestinationsBuilder;
        private SmtpClient _smtpClient;
        private MailboxAddress _fromAddress;
        private IRecipientProvider _recipientProvider;
        private ILogSerializer _mailSubjectGenerator;
        private ILogSerializer _mailBodyGenerator;
        private ILogSerializer _mailAttachmentGenerator;

        /// <summary>
        /// Creates an instance of <c>TacitusLogger.Destinations.Email.SmtpEmailDestinationBuilder</c> using parent <c>ILogGroupDestinationsBuilder</c> instance.
        /// </summary>
        /// <param name="logGroupDestinationsBuilder"></param>
        public SmtpEmailDestinationBuilder(ILogGroupDestinationsBuilder logGroupDestinationsBuilder)
        {
            _logGroupDestinationsBuilder = logGroupDestinationsBuilder;
        }

        /// <summary>
        /// Gets log group destinations builder that was specified during the build process.
        /// </summary>
        public ILogGroupDestinationsBuilder LogGroupDestinationsBuilder => _logGroupDestinationsBuilder;
        /// <summary>
        /// Gets SMTP client that was specified during the build process.
        /// </summary>
        public SmtpClient SmtpClient => _smtpClient;
        /// <summary>
        /// Gets from address that was specified during the build process.
        /// </summary>
        public MailboxAddress FromAddress => _fromAddress;
        /// <summary>
        /// Gets recipient provider that was specified during the build process.
        /// </summary>
        public IRecipientProvider RecipientProvider => _recipientProvider;
        /// <summary>
        /// Gets mail subject generator that was specified during the build process.
        /// </summary>
        public ILogSerializer MailSubjectGenerator => _mailSubjectGenerator;
        /// <summary>
        /// Gets mail body generator that was specified during the build process.
        /// </summary>
        public ILogSerializer MailBodyGenerator => _mailBodyGenerator;
        /// <summary>
        /// Gets mail attachment generator that was specified during the build process.
        /// </summary>
        public ILogSerializer MailAttachmentGenerator => _mailAttachmentGenerator;

        /// <summary>
        /// Adds the SMTP client of type <c>System.Net.Mail.SmtpClient</c> and 
        /// address of type <c>System.Net.Mail.MailboxAddress</c> to the builder.
        /// </summary>
        /// <param name="smtpClient"></param>
        /// <returns></returns>
        public ISmtpEmailDestinationBuilder WithSmtpClient(SmtpClient smtpClient, MailboxAddress fromAddress)
        {
            if (_smtpClient != null)
                throw new InvalidOperationException("SMTP client has already been specified during the build process");
            if (_fromAddress != null)
                throw new InvalidOperationException("From address has already been specified during the build process");

            _smtpClient = smtpClient ?? throw new ArgumentNullException("smtpClient");
            _fromAddress = fromAddress ?? throw new ArgumentNullException("fromAddress");
            return this;
        }
        /// <summary>
        /// Adds to the builder the SMTP client of type <c>System.Net.Mail.SmtpClient</c> and 
        /// from address string that will be used to create from address of type <c>System.Net.Mail.MailboxAddress</c>.
        /// </summary>
        /// <param name="smtpClient"></param>
        /// <returns></returns>
        public ISmtpEmailDestinationBuilder WithSmtpClient(SmtpClient smtpClient, string fromAddress)
        {
            if (_smtpClient != null)
                throw new InvalidOperationException("SMTP client has already been specified during the build process");
            if (_fromAddress != null)
                throw new InvalidOperationException("From address has already been specified during the build process");
            _smtpClient = smtpClient ?? throw new ArgumentNullException("smtpClient");
            _fromAddress = new MailboxAddress(fromAddress ?? throw new ArgumentNullException("fromAddress"));
            return this;
        }
        /// <summary>
        /// Adds custom recipient provider of type <c>TacitusLogger.Destinations.Email.IRecipientProvider</c> to the builder.
        /// </summary>
        /// <param name="recipientProvider"></param>
        /// <returns></returns>
        public ISmtpEmailDestinationBuilder WithRecipients(IRecipientProvider recipientProvider)
        {
            if (_recipientProvider != null)
                throw new InvalidOperationException("Recipient provider has already been specified during the build process");
            _recipientProvider = recipientProvider ?? throw new ArgumentNullException("recipientProvider");
            return this;
        }
        /// <summary>
        /// Adds a custom log serializer of type <c>TacitusLogger.Serializers.ILogSerializer</c>
        /// that will be used to generate mail subject text to the builder. If omitted, the default mail subject generator
        /// of type <c>TacitusLogger.Serializers.SimpleTemplateLogSerializer</c> with the default template will be added.
        /// </summary>
        /// <param name="mailSubjectGenerator"></param>
        /// <returns></returns>
        public ISmtpEmailDestinationBuilder WithSubject(ILogSerializer mailSubjectGenerator)
        {
            if (_mailSubjectGenerator != null)
                throw new InvalidOperationException("Mail subject generator has already been specified during the build process");
            _mailSubjectGenerator = mailSubjectGenerator ?? throw new ArgumentNullException("mailSubjectGenerator");
            return this;
        }
        /// <summary>
        /// Adds custom log serializer of type <c>TacitusLogger.Serializers.ILogSerializer</c>
        /// that will be used to generate mail body text to the builder. If omitted, the default mail body generator
        /// of type <c>TacitusLogger.Serializers.ExtendedTemplateLogSerializer</c> with default template will be added.
        /// </summary>
        /// <param name="mailBodyGenerator"></param>
        /// <returns></returns>
        public ISmtpEmailDestinationBuilder WithBody(ILogSerializer mailBodyGenerator)
        {
            if (_mailBodyGenerator != null)
                throw new InvalidOperationException("Mail body generator has already been specified during the build process");
            _mailBodyGenerator = mailBodyGenerator ?? throw new ArgumentNullException("mailBodyGenerator");
            return this;
        }
        /// <summary>
        /// Adds custom log serializer of type <c>TacitusLogger.Serializers.ILogSerializer</c>
        /// that will be used to generate mail attachment text to the builder. If you do not want the mails
        /// to contain the attachment, just omitted this stage.
        /// contain no 
        /// </summary>
        /// <param name="mailAttachmentContentGenerator"></param>
        /// <returns></returns>
        public ISmtpEmailDestinationBuilder WithAttachment(ILogSerializer mailAttachmentContentGenerator)
        {
            if (_mailAttachmentGenerator != null)
                throw new InvalidOperationException("Mail attachment generator has already been specified during the build process");
            _mailAttachmentGenerator = mailAttachmentContentGenerator ?? throw new ArgumentNullException("mailAttachmentContentGenerator");
            return this;
        }
        /// <summary>
        /// Completes log destination build process by adding it to the parent log group destination builder.
        /// </summary>
        /// <returns></returns>
        public ILogGroupDestinationsBuilder Add()
        {
            // Mandatory dependencies
            if (_smtpClient == null)
                throw new InvalidOperationException("SMTP client was not specified during the build");
            if (_fromAddress == null)
                throw new InvalidOperationException("From address was not specified during the build");
            if (_recipientProvider == null)
                throw new InvalidOperationException("Recipient provider was not specified during the build");

            // Dependencies with defaults
            if (_mailSubjectGenerator == null)
                _mailSubjectGenerator = new SimpleTemplateLogSerializer();
            if (_mailBodyGenerator == null)
                _mailBodyGenerator = new ExtendedTemplateLogSerializer();

            // Creating the destination
            EmailDestination emailDestination = new EmailDestination(_smtpClient, _fromAddress, _recipientProvider, _mailSubjectGenerator, _mailBodyGenerator, _mailAttachmentGenerator);

            // Adding the destination to the log group.
            return _logGroupDestinationsBuilder.CustomDestination(emailDestination);
        }
    }
}
