using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Domain.Events;

public record IdentityCreatedEvent(Identity identity) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public readonly Identity Identity = identity;
}

public record IdentityUpdatedEvent(Identity identity) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public readonly Identity Identity = identity;
}

public record IdentityDeletedEvent(Identity identity) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public readonly Identity Identity = identity;
}
