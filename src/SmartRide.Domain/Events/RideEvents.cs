using SmartRide.Domain.Entities;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Domain.Events;

public record RideCreatedEvent(Ride Ride) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record RideUpdatedEvent(Ride Ride) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record RideDeletedEvent(Ride Ride) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}