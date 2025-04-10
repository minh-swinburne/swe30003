using SmartRide.Domain.Enums;

namespace SmartRide.Application.DTOs.Rides;

public class CreateRideResponseDTO : BaseRideResponseDTO
{
    public required Guid PassengerId { get; set; }
    public required VehicleTypeEnum VehicleType { get; set; }
    public required RideTypeEnum RideType { get; set; }
    public required RideStatusEnum RideStatus { get; set; }
    public required Guid PickupLocationId { get; set; }
    public required Guid DestinationId { get; set; }
    public DateTime? PickupETA { get; set; }
    public DateTime? ArrivalETA { get; set; }
    public required decimal Fare { get; set; }
    public string? Notes { get; set; }
    public Guid? PaymentId { get; set; }
}
