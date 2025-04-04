using MediatR;

namespace SmartRide.Domain.Interfaces;

public interface IDomainEvent : INotification
{
    DateTime OccurredOn { get; }
}
