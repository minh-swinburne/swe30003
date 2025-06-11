using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Domain.Events;

public record VehicleCreatedEvent(Vehicle Vehicle) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public readonly Vehicle Vehicle = Vehicle;
}

public record VehicleUpdatedEvent(Vehicle Vehicle) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public readonly Vehicle Vehicle = Vehicle;
}

public record VehicleDeletedEvent(Vehicle Vehicle) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public readonly Vehicle Vehicle = Vehicle;
}
