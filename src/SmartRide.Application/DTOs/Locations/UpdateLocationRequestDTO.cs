namespace SmartRide.Application.DTOs.Locations;

public class UpdateLocationRequestDTO : BaseRequestDTO
{
    public required Guid LocationId { get; init; }
    public string? Address { get; init; }
    public double? Latitude { get; init; }
    public double? Longitude { get; init; }
    public Guid? UserId { get; init; }
}
