using Newtonsoft.Json;
using System.Collections.Generic;
using TacitusLogger.Serializers;

namespace TacitusLogger.Destinations.Email
{
    public static class ISmtpEmailDestinationBuilderExtensions
    {
        #region Extension methods related to WithRecipients method

        public static ISmtpEmailDestinationBuilder WithRecipients(this ISmtpEmailDestinationBuilder self, params string[] recipients)
        {
            return self.WithRecipients(new EmailListRecipientProvider(recipients));
        }
        public static ISmtpEmailDestinationBuilder WithRecipients(this ISmtpEmailDestinationBuilder self, LogModelFunc<ICollection<string>> factoryMethod)
        {
            return self.WithRecipients(new FactoryMethodRecipientProvider(factoryMethod));
        }

        #endregion

        #region Extension methods related to WithSubject method

        public static ISmtpEmailDestinationBuilder WithSubject(this ISmtpEmailDestinationBuilder self, string template)
        {
            return self.WithSubject(new SimpleTemplateLogSerializer(template));
        }
        public static ISmtpEmailDestinationBuilder WithSubject(this ISmtpEmailDestinationBuilder self, LogModelFunc<string> factoryMethod)
        {
            return self.WithSubject(new GeneratorFunctionLogSerializer(factoryMethod));
        }

        #endregion

        #region Extension methods related to WithBody method

        public static ISmtpEmailDestinationBuilder WithBody(this ISmtpEmailDestinationBuilder self, string template)
        {
            return self.WithBody(new ExtendedTemplateLogSerializer(template));
        }
        public static ISmtpEmailDestinationBuilder WithBody(this ISmtpEmailDestinationBuilder self, string template, JsonSerializerSettings jsonSerializerSettings)
        {
            return self.WithBody(new ExtendedTemplateLogSerializer(template, jsonSerializerSettings));
        }
        public static ISmtpEmailDestinationBuilder WithBody(this ISmtpEmailDestinationBuilder self, LogModelFunc<string> factoryMethod)
        {
            return self.WithBody(new GeneratorFunctionLogSerializer(factoryMethod));
        }

        #endregion

        #region Extension methods related to WithAttachment method

        public static ISmtpEmailDestinationBuilder WithAttachment(this ISmtpEmailDestinationBuilder self, string template)
        {
            return self.WithAttachment(new ExtendedTemplateLogSerializer(template));
        }
        public static ISmtpEmailDestinationBuilder WithAttachment(this ISmtpEmailDestinationBuilder self, string template, JsonSerializerSettings jsonSerializerSettings)
        {
            return self.WithAttachment(new ExtendedTemplateLogSerializer(template, jsonSerializerSettings));
        }
        public static ISmtpEmailDestinationBuilder WithAttachment(this ISmtpEmailDestinationBuilder self, LogModelFunc<string> factoryMethod)
        {
            return self.WithAttachment(new GeneratorFunctionLogSerializer(factoryMethod));
        }

        #endregion
    }
}
