# TacitusLogger.Destinations.Email

> Extension destination for TacitusLogger that sends logs as emails using SMTP protocol.
 
Dependencies:  
* NET Standard >= 1.3  
* TacitusLogger >= 0.2.0  
* jstedfast/MailKit >= 2.0.0
  
> Attention: `TacitusLogger.Destinations.Email` is currently in **Alpha phase**. This means you should not use it in any production code.

## Installation

The NuGet <a href="http://example.com/" target="_blank">package</a>:

```powershell
PM> Install-Package TacitusLogger.Destinations.Email
```

## Examples

### Adding email destination with minimal configuration
With builders:
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
With builders:
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
With builders:
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
With builders:
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
With builders:
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
With builders:
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
With builders:
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
With builders:
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
With builders:
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




