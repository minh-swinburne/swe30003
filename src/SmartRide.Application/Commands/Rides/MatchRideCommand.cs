using SmartRide.Application.DTOs.Rides;

namespace SmartRide.Application.Commands.Rides;

public class MatchRideCommand : BaseCommand<MatchRideResponseDTO>
{
    public Guid RideId { get; set; }
    public Guid DriverId { get; set; }
    public Guid VehicleId { get; set; }
    public DateTime PickupETA { get; set; }
}