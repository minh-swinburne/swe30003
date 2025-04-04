namespace SmartRide.Domain.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
    Task SendEmailWithAttachmentAsync(string to, string subject, string body, string attachmentPath);
    Task SendBulkEmailAsync(IEnumerable<string> to, string subject, string body);
    Task SendEmailWithTemplateAsync(string to, string templateName, object model);
    Task SendEmailWithTemplateAndAttachmentAsync(string to, string templateName, object model, string attachmentPath);
}
