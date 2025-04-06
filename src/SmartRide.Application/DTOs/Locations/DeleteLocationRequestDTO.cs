namespace SmartRide.Application.DTOs.Locations;

public class DeleteLocationRequestDTO : BaseRequestDTO
{
    public required Guid LocationId { get; init; }
}
