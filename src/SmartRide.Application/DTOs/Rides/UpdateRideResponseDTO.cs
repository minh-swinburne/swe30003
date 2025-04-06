using SmartRide.Domain.Enums;

namespace SmartRide.Application.DTOs.Rides;

public class UpdateRideResponseDTO : BaseRideResponseDTO
{
    public RideStatusEnum? RideStatus { get; set; }
    public DateTime? PickupETA { get; set; }
    public DateTime? ArrivalETA { get; set; }
    public decimal? Fare { get; set; }
    public string? Notes { get; set; }
}
