namespace SmartRide.Domain.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
    Task SendEmailWithAttachmentsAsync(string to, string subject, string body, List<string> attachmentPaths);
    Task SendBulkEmailAsync(IEnumerable<string> to, string subject, string body);
    Task SendEmailWithTemplateAsync(string to, string templateName, object model);
    Task SendEmailWithTemplateAndAttachmentsAsync(string to, string templateName, object model, List<string> attachmentPaths);
}
