using Microsoft.EntityFrameworkCore;
using SmartRide.Domain.Entities.Lookup;
using SmartRide.Domain.Enums;
using SmartRide.Domain.Events;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartRide.Domain.Entities.Base;

[Index(nameof(Vin), IsUnique = true)]
[Index(nameof(Plate), IsUnique = true)]
public class Vehicle : BaseEntity
{
    [Required]
    [Column(TypeName = "BINARY(16)", Order = 0)]
    public required Guid UserId { get; set; }

    [Required]
    [Column(TypeName = "TINYINT", Order = 1)]
    public required VehicleTypeEnum VehicleTypeId { get; set; }

    [Required]
    [Column(TypeName = "CHAR")]
    [StringLength(17)]
    public required string Vin { get; set; }

    [Required]
    [Column(TypeName = "VARCHAR")]
    [StringLength(10)]
    public required string Plate { get; set; }

    [Required]
    [Column(TypeName = "VARCHAR")]
    [StringLength(50)]
    public required string Make { get; set; }

    [Required]
    [Column(TypeName = "VARCHAR")]
    [StringLength(50)]
    public required string Model { get; set; }

    [Required]
    [Column(TypeName = "INT")]
    public required int Year { get; set; }

    [Required]
    [Column(TypeName = "DATE")]
    public required DateTime RegisteredDate { get; set; }

    [Required]
    [ForeignKey(nameof(UserId))]    // can be removed if the property name is UserId
    public User User { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(VehicleTypeId))]
    public VehicleType VehicleType { get; set; } = null!;

    public void ValidateDriverRole()
    {
        if (User == null || !User.IsDriver())
        {
            throw new InvalidOperationException("Vehicle can only be assigned to a driver.");
        }
    }

    public override void OnSave(EntityState state)
    {
        base.OnSave(state);

        // Add domain events based on the entity state
        if (state == EntityState.Added)
        {
            ValidateDriverRole();
            AddDomainEvent(new VehicleCreatedEvent(this));
        }
        else if (state == EntityState.Modified)
        {
            AddDomainEvent(new VehicleUpdatedEvent(this));
        }
        else if (state == EntityState.Deleted)
        {
            AddDomainEvent(new VehicleDeletedEvent(this));
        }
    }
}
