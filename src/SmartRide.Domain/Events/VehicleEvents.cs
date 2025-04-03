using SmartRide.Domain.Entities;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Domain.Events;

public record VehicleCreatedEvent(Vehicle Vehicle) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record VehicleUpdatedEvent(Vehicle Vehicle) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record VehicleDeletedEvent(Vehicle Vehicle) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
