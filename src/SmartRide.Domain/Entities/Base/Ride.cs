using Microsoft.EntityFrameworkCore;
using SmartRide.Common.Constants;
using SmartRide.Domain.Enums;
using SmartRide.Domain.Events;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartRide.Domain.Entities.Base;

public class Ride : BaseEntity
{
    [Required]
    [Column(TypeName = "BINARY(16)")]
    public required Guid PassengerId { get; init; }

    [Column(TypeName = "BINARY(16)")]
    public Guid? DriverId { get; init; }

    [Column(TypeName = "BINARY(16)")]
    public Guid? VehicleId { get; init; }

    [Required]
    [Column(TypeName = "TINYINT")]
    public required RideTypeEnum Type { get; init; }

    [Required]
    [Column(TypeName = "TINYINT")]
    public RideStatusEnum Status { get; set; } = RideStatusEnum.Pending;

    [Required]
    [Column(TypeName = "BINARY(16)")]
    public required Guid PickupLocationId { get; set; }

    [Required]
    [Column(TypeName = "BINARY(16)")]
    public required Guid DestinationId { get; set; }

    [Column(TypeName = "DATETIME")]
    public DateTime? PickupETA { get; set; }

    [Column(TypeName = "DATETIME")]
    public DateTime? PickupATA { get; set; }

    [Column(TypeName = "DATETIME")]
    public DateTime? ArrivalETA { get; set; }

    [Column(TypeName = "DATETIME")]
    public DateTime? ArrivalATA { get; set; }

    [Required]
    [Range((double)RideConstants.MinFare, (double)RideConstants.MaxFare)]
    [Column(TypeName = "DECIMAL(18,2)")]
    public decimal Fare { get; set; }

    [Column(TypeName = "TEXT")]
    [StringLength(RideConstants.NotesMaxLength)]
    public string? Notes { get; set; }

    [Required]
    [ForeignKey(nameof(PassengerId))]
    public User Passenger { get; set; } = null!;

    [ForeignKey(nameof(DriverId))]
    public User? Driver { get; set; }

    [ForeignKey(nameof(VehicleId))]
    public Vehicle? Vehicle { get; set; }

    [Required]
    [ForeignKey(nameof(PickupLocationId))]
    public Location PickupLocation { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(DestinationId))]
    public Location Destination { get; set; } = null!;

    public Payment? Payment { get; set; }

    public Feedback? Feedback { get; set; }

    public void ValidateDriverRole()
    {
        if (Driver != null && !Driver.IsDriver())
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
        if (Vehicle != null && Vehicle.UserId != DriverId)
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