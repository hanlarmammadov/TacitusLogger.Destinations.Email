# TacitusLogger.Destinations.Email

> Extension destination for TacitusLogger that sends logs as emails using SMTP protocol.
 
Dependencies:  
* NET Standard >= 1.3  
* TacitusLogger >= 0.2.0  
* jstedfast/MailKit >= 2.0.0
  
> Attention: `TacitusLogger.Destinations.Email` is currently in **Alpha phase**. This means you should not use it in any production code.

## Installation

The NuGet <a href="https://www.nuget.org/packages/TacitusLogger.Destinations.Email" target="_blank">package</a>:

```powershell
PM> Install-Package TacitusLogger.Destinations.Email
```

## Examples

### Adding email destination with minimal configuration
Using builders:
```cs
var logger = LoggerBuilder.Logger()
                          .ForAllLogs()
                          .Email()
                              .WithRecipients("recipient@example.com")
                              .WithSmtpClient(mailKitSmtpClient, "sender@example.com")
                              .Add()
                          .BuildLogger();
```
Directly:
```cs
MailboxAddress fromAddress = new MailboxAddress("sender", "sender@example.com");
EmailDestination emailDestination = new EmailDestination(mailKitSmtpClient, fromAddress, "recipient@example.com");

Logger logger = new Logger();
logger.AddLogDestinations(emailDestination);
```
---
### With several recipients
Using builders:
```cs
var recipients = new string[] 
{
    "recipient@example.com",
    "recipient@example.com",
    "recipient@example.com"
};
var logger = LoggerBuilder.Logger()
                          .ForAllLogs()
                          .Email()
                              .WithRecipients(recipients)
                              .WithSmtpClient(mailKitSmtpClient, "sender@example.com")
                              .Add()
                          .BuildLogger(); 
```
Directly:
```cs
var recipients = new string[]
{
    "recipient@example.com",
    "recipient@example.com",
    "recipient@example.com"
};
MailboxAddress fromAddress = new MailboxAddress("sender", "sender@example.com");
EmailDestination emailDestination = new EmailDestination(mailKitSmtpClient, fromAddress, recipients);

Logger logger = new Logger();
logger.AddLogDestinations(emailDestination);
```
---
### With recipients function
Using builders:
```cs
LogModelFunc<ICollection<string>> recipientsFunc = (logModel) =>
{
    if (logModel.LogTypeIsIn(LogType.Error, LogType.Failure, LogType.Critical))
        return new string[] { "recipient1@example.com" };
    else
        return new string[] { "recipient2@example.com" };
};
var logger = LoggerBuilder.Logger()
                          .ForAllLogs()
                          .Email()
                              .WithRecipients(recipientsFunc)
                              .WithSmtpClient(mailKitSmtpClient, "sender@example.com")
                              .Add()
                          .BuildLogger();
```
Directly:
```cs
FactoryMethodRecipientProvider factoryMethodRecipientProvider = new FactoryMethodRecipientProvider((logModel) =>
{
    if (logModel.LogTypeIsIn(LogType.Error, LogType.Failure, LogType.Critical))
        return new string[] { "recipient1@example.com" };
    else
        return new string[] { "recipient2@example.com" };
});
MailboxAddress fromAddress = new MailboxAddress("sender", "sender@example.com");
EmailDestination emailDestination = new EmailDestination(mailKitSmtpClient, fromAddress, factoryMethodRecipientProvider);

Logger logger = new Logger();
logger.AddLogDestinations(emailDestination);
```
---
### With custom recipient provider
Using builders:
```cs
IRecipientProvider customRecipientProvider = new Mock<IRecipientProvider>().Object;
var logger = LoggerBuilder.Logger()
                          .ForAllLogs()
                          .Email()
                              .WithRecipients(customRecipientProvider)
                              .WithSmtpClient(mailKitSmtpClient, "sender@example.com")
                              .Add()
                          .BuildLogger();
```
Directly:
```cs
IRecipientProvider customRecipientProvider = new Mock<IRecipientProvider>().Object;
MailboxAddress fromAddress = new MailboxAddress("sender", "sender@example.com");
EmailDestination emailDestination = new EmailDestination(mailKitSmtpClient, fromAddress, customRecipientProvider);

Logger logger = new Logger();
logger.AddLogDestinations(emailDestination);
```
---
### With mailbox address
Using builders:
```cs
MailboxAddress fromAddress = new MailboxAddress("sender", "sender@example.com");
var logger = LoggerBuilder.Logger()
                          .ForAllLogs()
                          .Email()
                              .WithRecipients("recipient@example.com")
                              .WithSmtpClient(mailKitSmtpClient, fromAddress)
                              .Add()
                          .BuildLogger();
```
---
### With custom subject body and attachment templates
Using builders:
```cs
var logger = LoggerBuilder.Logger()
                          .ForAllLogs()
                          .Email()
                              .WithRecipients("recipient@example.com")
                              .WithSmtpClient(mailKitSmtpClient, "sender@example.com")
                              .WithSubject("Notification from logger $Source: $LogType - $Description")
                              .WithBody(customBodyTemplate)
                              .WithAttachment(customAttachmentTemplate)
                              .Add()
                          .BuildLogger();
```
Directly:
```cs
EmailListRecipientProvider emailListRecipientProvider = new EmailListRecipientProvider("recipient@example.com");
SimpleTemplateLogSerializer subjectTextSerializer = new SimpleTemplateLogSerializer("Notification from logger $Source: $LogType - $Description");
ExtendedTemplateLogSerializer bodyLogSerializer = new ExtendedTemplateLogSerializer(customBodyTemplate);
ExtendedTemplateLogSerializer attachmentLogSerializer = new ExtendedTemplateLogSerializer(customAttachmentTemplate);
MailboxAddress fromAddress = new MailboxAddress("sender", "sender@example.com");

EmailDestination emailDestination = new EmailDestination(mailKitSmtpClient,
                                                            fromAddress,
                                                            emailListRecipientProvider,
                                                            subjectTextSerializer,
                                                            bodyLogSerializer,
                                                            attachmentLogSerializer);
Logger logger = new Logger();
logger.AddLogDestinations(emailDestination);
```
---
### With custom body and attachment templates and json serializer settings
Using builders:
```cs
var jsonSerializerSettings = new JsonSerializerSettings();
var logger = LoggerBuilder.Logger()
                          .ForAllLogs()
                          .Email()
                              .WithRecipients("recipient@example.com")
                              .WithSmtpClient(mailKitSmtpClient, "sender@example.com")
                              .WithSubject("Notification from logger $Source: $LogType - $Description")
                              .WithBody(customBodyTemplate, jsonSerializerSettings)
                              .WithAttachment(customAttachmentTemplate, jsonSerializerSettings)
                              .Add()
                          .BuildLogger();
```
Directly:
```cs
var jsonSerializerSettings = new JsonSerializerSettings();
EmailListRecipientProvider emailListRecipientProvider = new EmailListRecipientProvider("recipient@example.com");
SimpleTemplateLogSerializer subjectTextSerializer = new SimpleTemplateLogSerializer("Notification from logger $Source: $LogType - $Description");
ExtendedTemplateLogSerializer bodyLogSerializer = new ExtendedTemplateLogSerializer(customBodyTemplate, jsonSerializerSettings);
ExtendedTemplateLogSerializer attachmentLogSerializer = new ExtendedTemplateLogSerializer(customAttachmentTemplate, jsonSerializerSettings);
MailboxAddress fromAddress = new MailboxAddress("sender", "sender@example.com");

EmailDestination emailDestination = new EmailDestination(mailKitSmtpClient,
                                                            fromAddress,
                                                            emailListRecipientProvider,
                                                            subjectTextSerializer,
                                                            bodyLogSerializer,
                                                            attachmentLogSerializer);
Logger logger = new Logger();
logger.AddLogDestinations(emailDestination);
```
---
### With custom subject body and attachment serializers
Using builders:
```cs
ILogSerializer customSubjectSerializer = new Mock<ILogSerializer>().Object;
ILogSerializer customBodySerializer = new Mock<ILogSerializer>().Object;
ILogSerializer customAttachmentSerializer = new Mock<ILogSerializer>().Object;

var logger = LoggerBuilder.Logger()
                          .ForAllLogs()
                          .Email()
                              .WithRecipients("recipient@example.com")
                              .WithSmtpClient(mailKitSmtpClient, "sender@example.com")
                              .WithSubject(customSubjectSerializer)
                              .WithBody(customBodySerializer)
                              .WithAttachment(customAttachmentSerializer)
                              .Add()
                          .BuildLogger();
```
Directly:
```cs
ILogSerializer customSubjectSerializer = new Mock<ILogSerializer>().Object;
ILogSerializer customBodySerializer = new Mock<ILogSerializer>().Object;
ILogSerializer customAttachmentSerializer = new Mock<ILogSerializer>().Object;
MailboxAddress fromAddress = new MailboxAddress("sender", "sender@example.com");
EmailListRecipientProvider emailListRecipientProvider = new EmailListRecipientProvider("recipient@example.com");

EmailDestination emailDestination = new EmailDestination(mailKitSmtpClient,
                                                            fromAddress,
                                                            emailListRecipientProvider,
                                                            customSubjectSerializer,
                                                            customBodySerializer,
                                                            customAttachmentSerializer);
Logger logger = new Logger();
logger.AddLogDestinations(emailDestination);
```
---
### With custom subject text function 
Using builders:
```cs
var logger = LoggerBuilder.Logger()
                          .ForAllLogs()
                          .Email()
                              .WithRecipients("recipient@example.com")
                              .WithSmtpClient(mailKitSmtpClient, "sender@example.com")
                              .WithSubject(m => $"Notification from logger {m.Source}: {m.LogType} - {m.Description}")
                              .Add()
                          .BuildLogger();
```
Directly:
```cs
GeneratorFunctionLogSerializer subjectTextFunction = new GeneratorFunctionLogSerializer(m =>
{
    return $"Notification from logger {m.Source}: {m.LogType} - {m.Description}";
}); 
MailboxAddress fromAddress = new MailboxAddress("sender", "sender@example.com");
EmailListRecipientProvider emailListRecipientProvider = new EmailListRecipientProvider("recipient@example.com");

EmailDestination emailDestination = new EmailDestination(mailKitSmtpClient,
                                                            fromAddress,
                                                            emailListRecipientProvider,
                                                            subjectTextFunction);
Logger logger = new Logger();
logger.AddLogDestinations(emailDestination);
``` 

## License

[APACHE LICENSE 2.0](https://www.apache.org/licenses/LICENSE-2.0)

## See also

TacitusLogger:  

- [TacitusLogger](https://github.com/khanlarmammadov/TacitusLogger) - A simple yet powerful .NET logging library.

Destinations:

- [TacitusLogger.Destinations.MongoDb](https://github.com/khanlarmammadov/TacitusLogger.Destinations.MongoDb) - Extension destination for TacitusLogger that sends logs to MongoDb database.
- [TacitusLogger.Destinations.RabbitMq](https://github.com/khanlarmammadov/TacitusLogger.Destinations.RabbitMq) - Extension destination for TacitusLogger that sends logs to the RabbitMQ exchanges.
- [TacitusLogger.Destinations.EntityFramework](https://github.com/khanlarmammadov/TacitusLogger.Destinations.EntityFramework) - Extension destination for TacitusLogger that sends logs to database using Entity Framework ORM.
- [TacitusLogger.Destinations.Trace](https://github.com/khanlarmammadov/TacitusLogger.Destinations.Trace) - Extension destination for TacitusLogger that sends logs to System.Diagnostics.Trace listeners.  
  
Dependency injection:
- [TacitusLogger.DI.Ninject](https://github.com/khanlarmammadov/TacitusLogger.DI.Ninject) - Extension for Ninject dependency injection container that helps to configure and add TacitusLogger as a singleton.
- [TacitusLogger.DI.Autofac](https://github.com/khanlarmammadov/TacitusLogger.DI.Autofac) - Extension for Autofac dependency injection container that helps to configure and add TacitusLogger as a singleton.
- [TacitusLogger.DI.MicrosoftDI](https://github.com/khanlarmammadov/TacitusLogger.DI.MicrosoftDI) - Extension for Microsoft dependency injection container that helps to configure and add TacitusLogger as a singleton.  

Log contributors:

- [TacitusLogger.Contributors.ThreadInfo](https://github.com/khanlarmammadov/TacitusLogger.Contributors.ThreadInfo) - Extension contributor for TacitusLogger that generates additional info related to the thread on which the logger method was called.
- [TacitusLogger.Contributors.MachineInfo](https://github.com/khanlarmammadov/TacitusLogger.Contributors.MachineInfo) - Extension contributor for TacitusLogger that generates additional info related to the machine on which the log was produced.