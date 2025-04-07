using SmartRide.Application.DTOs.Rides;
using SmartRide.Domain.Enums;

namespace SmartRide.Application.Commands.Rides;

public class CreateRideCommand : BaseCommand<CreateRideResponseDTO>
{
    public Guid PassengerId { get; set; }
    public RideTypeEnum RideType { get; set; }
    public VehicleTypeEnum VehicleType { get; set; }
    public Guid PickupLocationId { get; set; }
    public Guid DestinationId { get; set; }
    public DateTime? PickupETA { get; set; }
    public DateTime? ArrivalETA { get; set; }
    public decimal Fare { get; set; }
    public string? Notes { get; set; }
}
