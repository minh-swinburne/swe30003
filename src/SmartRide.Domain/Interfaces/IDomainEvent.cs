namespace SmartRide.Domain.Interfaces;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}
