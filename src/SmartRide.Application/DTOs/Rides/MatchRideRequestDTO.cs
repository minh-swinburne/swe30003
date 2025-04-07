using SmartRide.Domain.Enums;

namespace SmartRide.Application.DTOs.Rides;

public class MatchRideRequestDTO : BaseRequestDTO
{
    public required Guid RideId { get; set; }
    public required Guid DriverId { get; set; }
    public required Guid VehicleId { get; set; }
}
