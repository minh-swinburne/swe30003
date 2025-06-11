using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Domain.Events;

public record LocationCreatedEvent(Location Location) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public readonly Location Location = Location;
}

public record LocationUpdatedEvent(Location Location) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public readonly Location Location = Location;
}

public record LocationDeletedEvent(Location Location) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public readonly Location Location = Location;
}
