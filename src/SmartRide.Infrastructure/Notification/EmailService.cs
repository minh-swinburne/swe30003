using SmartRide.Domain.Interfaces;

namespace SmartRide.Infrastructure.Notification;

public class EmailService : IEmailService
{
    // private readonly IEmailSender _emailSender;

    // public EmailService(IEmailSender emailSender)
    // {
    //     _emailSender = emailSender;
    // }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        // var message = new Message(new[] { to }, subject, body);
        // await _emailSender.SendEmailAsync(message);
    }

    public async Task SendEmailWithAttachmentsAsync(string to, string subject, string body, List<string> attachmentPaths)
    {
        // var message = new Message(new[] { to }, subject, body, attachmentPaths);
        // await _emailSender.SendEmailAsync(message);
    }

    public async Task SendBulkEmailAsync(IEnumerable<string> to, string subject, string body)
    {
        // var message = new Message(to.ToArray(), subject, body);
        // await _emailSender.SendEmailAsync(message);
    }

    public async Task SendEmailWithTemplateAsync(string to, string templateName, object model)
    {
        // var body = await RenderTemplateAsync(templateName, model);
        // var message = new Message(new[] { to }, "Subject", body);
        // await _emailSender.SendEmailAsync(message);
    }

    public async Task SendEmailWithTemplateAndAttachmentsAsync(string to, string templateName, object model, List<string> attachmentPaths)
    {
        // var body = await RenderTemplateAsync(templateName, model);
        // var message = new Message(new[] { to }, "Subject", body, attachmentPaths);
        // await _emailSender.SendEmailAsync(message);
    }
}
