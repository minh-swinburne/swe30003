using MediatR;
using SmartRide.Domain.Events;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Infrastructure.Events.Users;

public class WelcomeEmailHandler(IEmailService emailService) : INotificationHandler<UserCreatedEvent>
{
    private readonly IEmailService _emailService = emailService;

    public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        // Handle the event (e.g., send an email, log the event, etc.)
        var user = notification.User;
        var subject = "Welcome to SmartRide!";
        var body = $"Hello {user.FirstName},\n\nWelcome to SmartRide! We're glad to have you on board.\n\nBest regards,\nSmartRide Team";

        // Send the email using the email service
        await _emailService.SendEmailAsync(user.Email, subject, body);

        // We can also log the event or perform other actions as needed
        // For example, we could log the event to a database or an external service
        // await _eventLogger.LogEventAsync(notification, cancellationToken);
    }
}
