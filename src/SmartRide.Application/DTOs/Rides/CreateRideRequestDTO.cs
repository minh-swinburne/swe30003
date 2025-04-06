using SmartRide.Domain.Enums;

namespace SmartRide.Application.DTOs.Rides;

public class CreateRideRequestDTO : BaseRequestDTO
{
    public required Guid PassengerId { get; set; }
    public required Guid PickupLocationId { get; set; }
    public required Guid DestinationId { get; set; }
    public required RideTypeEnum RideType { get; set; }
    public required PaymentMethodEnum PaymentMethod { get; set; }
    public string? Notes { get; set; }
}
