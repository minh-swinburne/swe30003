namespace SmartRide.Application.DTOs.Locations;

public class GetLocationByIdRequestDTO : BaseRequestDTO
{
    public required Guid LocationId { get; init; }
}
