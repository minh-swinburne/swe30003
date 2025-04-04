using MediatR;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Domain.Events;

public record LicenseCreatedEvent(License License) : IDomainEvent, INotification
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record LicenseUpdatedEvent(License License) : IDomainEvent, INotification
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record LicenseDeletedEvent(License License) : IDomainEvent, INotification
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
