namespace SmartRide.Application.DTOs.Locations;

public class CreateLocationRequestDTO : BaseRequestDTO
{
    public required string Address { get; init; }
    public double? Latitude { get; init; }
    public double? Longitude { get; init; }
    public Guid? UserId { get; init; }
}
