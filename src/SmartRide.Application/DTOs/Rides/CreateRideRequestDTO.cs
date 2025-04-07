using SmartRide.Domain.Enums;

namespace SmartRide.Application.DTOs.Rides;

public class CreateRideRequestDTO : BaseRequestDTO
{
    public required Guid PassengerId { get; set; }

    // Allow both address and coordinates for pickup and destination
    public string? PickupAddress { get; set; }
    public double? PickupLatitude { get; set; }
    public double? PickupLongitude { get; set; }

    public string? DestinationAddress { get; set; }
    public double? DestinationLatitude { get; set; }
    public double? DestinationLongitude { get; set; }

    public required VehicleTypeEnum VehicleType { get; set; }
    public required RideTypeEnum RideType { get; set; }
    public required PaymentMethodEnum PaymentMethod { get; set; }
    public string? Notes { get; set; }
}
