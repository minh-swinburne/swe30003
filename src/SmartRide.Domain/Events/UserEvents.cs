using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Domain.Events;

public record UserCreatedEvent(User User) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record UserUpdatedEvent(User User) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record UserDeletedEvent(User User) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
