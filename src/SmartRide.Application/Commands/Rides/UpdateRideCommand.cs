using SmartRide.Application.DTOs.Rides;
using SmartRide.Domain.Enums;

namespace SmartRide.Application.Commands.Rides;

public class UpdateRideCommand : BaseCommand<UpdateRideResponseDTO>
{
    public Guid RideId { get; set; }
    public Guid? DriverId { get; set; }
    public Guid? VehicleId { get; set; }
    public RideStatusEnum? RideStatus { get; set; }
    public Guid? PickupLocationId { get; set; }
    public Guid? DestinationId { get; set; }
    public DateTime? PickupETA { get; set; }
    public DateTime? PickupATA { get; set; }
    public DateTime? ArrivalETA { get; set; }
    public DateTime? ArrivalATA { get; set; }
    public decimal? Fare { get; set; }
    public string? Notes { get; set; }
}
