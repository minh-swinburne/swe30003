using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Domain.Events;

public record UserCreatedEvent(User User) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public readonly User User = User;
}

public record UserUpdatedEvent(User User) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public readonly User User = User;
}

public record UserDeletedEvent(User User) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public readonly User User = User;
}
