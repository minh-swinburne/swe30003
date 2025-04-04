using Microsoft.EntityFrameworkCore;
using SmartRide.Domain.Enums;
using SmartRide.Domain.Events;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartRide.Domain.Entities.Base;

public class Ride : BaseEntity
{
    [Required]
    [Column(TypeName = "BINARY(16)")]
    public Guid PassengerId { get; init; }

    [Required]
    [Column(TypeName = "BINARY(16)")]
    public Guid DriverId { get; init; }

    [Required]
    [Column(TypeName = "BINARY(16)")]
    public Guid VehicleId { get; init; }

    [Required]
    [Column(TypeName = "TINYINT")]
    public RideTypeEnum Type { get; init; }

    [Column(TypeName = "TINYINT")]
    public RideStatusEnum Status { get; set; } = RideStatusEnum.Pending;

    [Required]
    [Column(TypeName = "BINARY(16)")]
    public Guid PickupLocationId { get; set; }

    [Required]
    [Column(TypeName = "BINARY(16)")]
    public Guid DestinationId { get; set; }

    [Column(TypeName = "DATETIME")]
    public DateTime? PickupETA { get; set; }

    [Column(TypeName = "DATETIME")]
    public DateTime? PickupATA { get; set; }

    [Column(TypeName = "DATETIME")]
    public DateTime? ArrivalETA { get; set; }

    [Column(TypeName = "DATETIME")]
    public DateTime? ArrivalATA { get; set; }

    [Required]
    [Range(0, (double)decimal.MaxValue)]
    [Column(TypeName = "DECIMAL(18,2)")]
    public float Fare { get; set; } // = .0f;

    [Column(TypeName = "TEXT")]
    public string? Notes { get; set; }

    [ForeignKey(nameof(PassengerId))]
    public User Passenger { get; set; } = null!;

    [ForeignKey(nameof(DriverId))]
    public User Driver { get; set; } = null!;

    [ForeignKey(nameof(VehicleId))]
    public Vehicle Vehicle { get; set; } = null!;

    [ForeignKey(nameof(PickupLocationId))]
    public Location PickupLocation { get; set; } = null!;

    [ForeignKey(nameof(DestinationId))]
    public Location Destination { get; set; } = null!;

    public void ValidateDriverRole()
    {
        if (!Driver.IsDriver())
        {
            throw new InvalidOperationException("The specified DriverId does not reference a User with the Driver role.");
        }
    }

    public void ValidatePassengerRole()
    {
        if (!Passenger.IsPassenger())
        {
            throw new InvalidOperationException("The specified PassengerId does not reference a User with the Passenger role.");
        }
    }

    public void ValidateVehicleOwnership()
    {
        if (Vehicle.UserId != DriverId)
        {
            throw new InvalidOperationException("The specified VehicleId does not reference a Vehicle owned by the specified DriverId.");
        }
    }

    public override void OnSave(EntityState state)
    {
        base.OnSave(state);

        // Add domain events based on the entity state
        if (state == EntityState.Added)
        {
            ValidateDriverRole();
            ValidatePassengerRole();
            ValidateVehicleOwnership();
            AddDomainEvent(new RideCreatedEvent(this));
        }
        else if (state == EntityState.Modified)
        {
            AddDomainEvent(new RideUpdatedEvent(this));
        }
        else if (state == EntityState.Deleted)
        {
            AddDomainEvent(new RideDeletedEvent(this));
        }
    }
}