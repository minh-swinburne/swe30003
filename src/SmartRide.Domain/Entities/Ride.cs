using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SmartRide.Domain.Enums;
using SmartRide.Domain.Events;

namespace SmartRide.Domain.Entities;

public class Ride : BaseEntity
{
    [Required]
    [Column(TypeName = "BINARY(16)")]
    public required Guid PassengerId { get; init; }

    [ForeignKey(nameof(PassengerId))]
    public required User Passenger { get; init; }

    [Required]
    [Column(TypeName = "BINARY(16)")]
    public required Guid DriverId { get; init; }

    [ForeignKey(nameof(DriverId))]
    public required User Driver { get; init; }

    [Required]
    [Column(TypeName = "BINARY(16)")]
    public required Guid VehicleId { get; init; }

    [ForeignKey(nameof(VehicleId))]
    public required Vehicle Vehicle { get; init; }

    [Required]
    [Column(TypeName = "TINYINT")]
    public required RideTypeEnum Type { get; init; }

    [Column(TypeName = "TINYINT")]
    public RideStatusEnum Status { get; set; } = RideStatusEnum.Pending;

    [Required]
    [Column(TypeName = "BINARY(16)")]
    public required Guid PickupLocationId { get; set; }

    [ForeignKey(nameof(PickupLocationId))]
    public required Location PickupLocation { get; set; }

    [Required]
    [Column(TypeName = "BINARY(16)")]
    public required Guid DestinationId { get; set; }

    [ForeignKey(nameof(DestinationId))]
    public required Location Destination { get; set; }

    [Column(TypeName = "DATETIME")]
    public DateTime? PickupETA { get; set; }

    [Column(TypeName = "DATETIME")]
    public DateTime? PickupATA { get; set; }

    [Column(TypeName = "DATETIME")]
    public DateTime? ArrivalETA { get; set; }

    [Column(TypeName = "DATETIME")]
    public DateTime? ArrivalATA { get; set; }

    [Column(TypeName = "FLOAT")]
    public float Fare { get; set; } = .0f;

    [Column(TypeName = "TEXT")]
    public string? Notes { get; set; }

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