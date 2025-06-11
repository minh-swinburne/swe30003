using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Domain.Events;

public record IdentityCreatedEvent(Identity Identity) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public readonly Identity Identity = Identity;
}

public record IdentityUpdatedEvent(Identity Identity) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public readonly Identity Identity = Identity;
}

public record IdentityDeletedEvent(Identity Identity) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public readonly Identity Identity = Identity;
}
