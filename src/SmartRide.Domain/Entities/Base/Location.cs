using SmartRide.Common.Constants;
using SmartRide.Domain.Events;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartRide.Domain.Entities.Base;

public class Location : BaseEntity
{
    [Required]
    [Column(TypeName = "VARCHAR")]
    [StringLength(LocationConstants.AddressMaxLength)]
    public required string Address { get; set; }

    [Column(TypeName = "DOUBLE")]
    public double? Latitude { get; set; }

    [Column(TypeName = "DOUBLE")]
    public double? Longitude { get; set; }

    [Column(TypeName = "BINARY(16)")]
    public Guid? UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }

    public override void OnSave(string state)
    {
        base.OnSave(state);

        if (state == "Added")
            AddDomainEvent(new LocationCreatedEvent(this));
        else if (state == "Modified")
            AddDomainEvent(new LocationUpdatedEvent(this));
        else if (state == "Deleted")
            AddDomainEvent(new LocationDeletedEvent(this));
    }
}
