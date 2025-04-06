using SmartRide.Application.DTOs.Locations;

namespace SmartRide.Application.Commands.Locations;

public class CreateLocationCommand : BaseCommand<CreateLocationResponseDTO>
{
    public string? Address { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public Guid? UserId { get; set; }
}
