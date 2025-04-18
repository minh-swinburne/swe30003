using SmartRide.Domain.Enums;

namespace SmartRide.Application.DTOs.Rides;

public class UpdateRideResponseDTO : BaseRideResponseDTO
{
    public required RideStatusEnum RideStatus { get; set; }
    public DateTime? PickupETA { get; set; }
    public DateTime? PickupATA { get; set; }
    public DateTime? ArrivalETA { get; set; }
    public DateTime? ArrivalATA { get; set; }
    public string? Notes { get; set; }
}
