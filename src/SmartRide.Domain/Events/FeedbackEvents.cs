using MediatR;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Domain.Events;

public record FeedbackCreatedEvent(Feedback Feedback) : IDomainEvent, INotification
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record FeedbackUpdatedEvent(Feedback Feedback) : IDomainEvent, INotification
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record FeedbackDeletedEvent(Feedback Feedback) : IDomainEvent, INotification
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
