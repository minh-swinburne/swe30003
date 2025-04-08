using SmartRide.Application.DTOs.Users;

namespace SmartRide.Application.DTOs.Locations;

public class GetLocationResponseDTO : BaseLocationResponseDTO
{
    public required string Address { get; init; }
    public double? Latitude { get; init; }
    public double? Longitude { get; init; }
    public GetUserResponseDTO? User { get; init; }
}
