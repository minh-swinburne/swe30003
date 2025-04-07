using SmartRide.Domain.Enums;

namespace SmartRide.Application.DTOs.Rides;

public class UpdateRideRequestDTO : BaseRequestDTO
{
    public required Guid RideId { get; set; }
    public Guid? DriverId { get; set; }
    public Guid? VehicleId { get; set; }
    public RideStatusEnum? RideStatus { get; set; }
    public DateTime? PickupATA { get; set; }
    public DateTime? ArrivalATA { get; set; }
    public string? Notes { get; set; }
}
