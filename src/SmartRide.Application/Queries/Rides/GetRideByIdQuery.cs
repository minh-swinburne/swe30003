using SmartRide.Application.DTOs.Rides;

namespace SmartRide.Application.Queries.Rides;

public class GetRideByIdQuery : BaseQuery<GetRideResponseDTO>
{
    public Guid RideId { get; init; }
}
