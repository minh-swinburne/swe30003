namespace SmartRide.Application.DTOs.Rides;

public class DeleteRideRequestDTO : BaseRequestDTO
{
    public required Guid RideId { get; set; }
}
