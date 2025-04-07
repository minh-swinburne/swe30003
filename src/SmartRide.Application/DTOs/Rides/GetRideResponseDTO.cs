using SmartRide.Domain.Enums;

namespace SmartRide.Application.DTOs.Rides;

public class GetRideResponseDTO : BaseRideResponseDTO
{
    public required Guid PassengerId { get; set; }
    public Guid? DriverId { get; set; }
    public Guid? VehicleId { get; set; }
    public required VehicleTypeEnum VehicleType { get; set; }
    public required RideTypeEnum RideType { get; set; }
    public required RideStatusEnum RideStatus { get; set; }
    public required Guid PickupLocationId { get; set; }
    public required Guid DestinationId { get; set; }
    public DateTime? PickupETA { get; set; }
    public DateTime? PickupATA { get; set; }
    public DateTime? ArrivalETA { get; set; }
    public DateTime? ArrivalATA { get; set; }
    public required decimal Fare { get; set; }
    public string? Notes { get; set; }
}
