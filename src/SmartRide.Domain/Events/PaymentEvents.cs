using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Domain.Events;

public record PaymentCreatedEvent(Payment Payment) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public readonly Payment Payment = Payment;
}

public record PaymentUpdatedEvent(Payment Payment) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public readonly Payment Payment = Payment;
}

public record PaymentDeletedEvent(Payment Payment) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public readonly Payment Payment = Payment;
}
