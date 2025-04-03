using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Domain.Entities;

public abstract class BaseEntity
{
    [Key]
    [Column(TypeName = "BINARY(16)")]
    public required Guid Id { get; set; }

    [Column(TypeName = "TIMESTAMP")]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime CreatedTime { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "TIMESTAMP")]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime UpdatedTime { get; set; } = DateTime.UtcNow;

    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public BaseEntity()
    {
        Id = Guid.NewGuid();
    }

    public virtual Guid GetId()
    {
        return Id;
    }

    public void UpdateTimestamp()
    {
        UpdatedTime = DateTime.UtcNow;
    }

    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public virtual void OnSave(EntityState state)
    {
        if (state == EntityState.Modified || state == EntityState.Added)
        {
            UpdateTimestamp();
        }
    }
}
