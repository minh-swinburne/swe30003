using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Domain.Events;

public record VehicleCreatedEvent(Vehicle vehicle) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public readonly Vehicle Vehicle = vehicle;
}

public record VehicleUpdatedEvent(Vehicle vehicle) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public readonly Vehicle Vehicle = vehicle;
}

public record VehicleDeletedEvent(Vehicle vehicle) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public readonly Vehicle Vehicle = vehicle;
}
