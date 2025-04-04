using MediatR;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Domain.Events;

public record PaymentCreatedEvent(Payment Payment) : IDomainEvent, INotification
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record PaymentUpdatedEvent(Payment Payment) : IDomainEvent, INotification
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record PaymentDeletedEvent(Payment Payment) : IDomainEvent, INotification
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
