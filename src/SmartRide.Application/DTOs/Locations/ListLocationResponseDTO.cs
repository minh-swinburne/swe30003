namespace SmartRide.Application.DTOs.Locations;

public class ListLocationResponseDTO : BaseLocationResponseDTO
{
    public required string Address { get; init; }
    public double? Latitude { get; init; }
    public double? Longitude { get; init; }
    public Guid? UserId { get; init; }
}
