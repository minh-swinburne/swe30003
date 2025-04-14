using Microsoft.EntityFrameworkCore;
using SmartRide.Common.Constants;
using SmartRide.Common.Exceptions;
using SmartRide.Common.Responses.Errors;
using SmartRide.Domain.Entities.Lookup;
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
    public required VehicleTypeEnum VehicleTypeId { get; set; }

    [Required]
    [Column(TypeName = "TINYINT")]
    public RideTypeEnum RideType { get; init; } = RideTypeEnum.Private;

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
    [Column(TypeName = "DECIMAL(20,4)")]
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
    [ForeignKey(nameof(VehicleTypeId))]
    public VehicleType VehicleType { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(PickupLocationId))]
    public Location PickupLocation { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(DestinationId))]
    public Location Destination { get; set; } = null!;

    public Payment Payment { get; set; } = null!;

    public Feedback? Feedback { get; set; }

    public void ValidateDriverRole()
    {
        if (Driver != null && !Driver.IsDriver())
        {
            throw new BaseException(
                RideErrors.Module,
                RideErrors.DRIVER_ROLE_INVALID.FormatMessage(("UserId", Driver.Id.ToString())),
                new InvalidOperationException(RideErrors.DRIVER_ROLE_INVALID.Message.Replace("{UserId}", DriverId.ToString()))
            );
        }
    }

    public void ValidatePassengerRole()
    {
        if (!Passenger.IsPassenger())
        {
            throw new BaseException(
                RideErrors.Module,
                RideErrors.PASSENGER_ROLE_INVALID.FormatMessage(("UserId", Passenger.Id.ToString())),
                new InvalidOperationException(RideErrors.PASSENGER_ROLE_INVALID.Message.Replace("{UserId}", PassengerId.ToString()))
            );
        }
    }

    public void ValidateVehicleOwnership()
    {
        if (Vehicle != null && Vehicle.UserId != DriverId)
        {
            throw new BaseException(
                RideErrors.Module,
                RideErrors.VEHICLE_OWNERSHIP_INVALID,
                new InvalidOperationException(RideErrors.VEHICLE_OWNERSHIP_INVALID.Message)
            );
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