using SmartRide.Domain.Enums;

namespace SmartRide.Application.DTOs.Rides;

public class UpdateRideResponseDTO : BaseRideResponseDTO
{
    public Guid? DriverId { get; set; }
    public Guid? VehicleId { get; set; }
    public RideStatusEnum? RideStatus { get; set; }
    public DateTime? PickupATA { get; set; }
    public DateTime? ArrivalATA { get; set; }
    public string? Notes { get; set; }
}
