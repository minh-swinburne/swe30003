using SmartRide.Domain.Enums;

namespace SmartRide.Application.DTOs.Rides;

public class MatchRideResponseDTO : BaseRideResponseDTO
{
    public required Guid DriverId { get; set; }
    public required Guid VehicleId { get; set; }
    public required RideStatusEnum RideStatus { get; set; }
    public DateTime? PickupETA { get; set; }
    public string? Notes { get; set; }
}
