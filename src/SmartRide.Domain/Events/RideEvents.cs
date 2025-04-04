using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Domain.Events;

public record RideCreatedEvent(Ride ride) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public readonly Ride Ride = ride;
}

public record RideUpdatedEvent(Ride ride) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public readonly Ride Ride = ride;
}

public record RideDeletedEvent(Ride ride) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public readonly Ride Ride = ride;
}