using MediatR;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Domain.Events;

public record LocationCreatedEvent(Location Location) : IDomainEvent, INotification
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record LocationUpdatedEvent(Location Location) : IDomainEvent, INotification
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record LocationDeletedEvent(Location Location) : IDomainEvent, INotification
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
