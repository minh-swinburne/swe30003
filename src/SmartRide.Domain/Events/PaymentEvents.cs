using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Domain.Events;

public record PaymentCreatedEvent(Payment payment) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public readonly Payment Payment = payment;
}

public record PaymentUpdatedEvent(Payment payment) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public readonly Payment Payment = payment;
}

public record PaymentDeletedEvent(Payment payment) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public readonly Payment Payment = payment;
}
