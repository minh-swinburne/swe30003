namespace SmartRide.Application.DTOs.Locations;

public class CreateLocationRequestDTO : BaseRequestDTO
{
    public required Guid UserId { get; init; }
    public string? Address { get; init; }
    public double? Latitude { get; init; }
    public double? Longitude { get; init; }
}
