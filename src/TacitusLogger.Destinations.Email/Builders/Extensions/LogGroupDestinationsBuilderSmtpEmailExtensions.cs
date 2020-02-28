using TacitusLogger.Builders;

namespace TacitusLogger.Destinations.Email
{
    public static class LogGroupDestinationsBuilderSmtpEmailExtensions
    {
        public static ISmtpEmailDestinationBuilder Email(this ILogGroupDestinationsBuilder obj)
        {
            return new SmtpEmailDestinationBuilder(obj);
        }
    }
}
