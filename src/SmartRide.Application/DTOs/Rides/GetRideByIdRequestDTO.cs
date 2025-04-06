namespace SmartRide.Application.DTOs.Rides;

public class GetRideByIdRequestDTO : BaseRequestDTO
{
    public required Guid RideId { get; init; }
}
