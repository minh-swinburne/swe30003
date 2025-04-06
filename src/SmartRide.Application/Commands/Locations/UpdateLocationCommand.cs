using SmartRide.Application.DTOs.Locations;

namespace SmartRide.Application.Commands.Locations;

public class UpdateLocationCommand : BaseCommand<UpdateLocationResponseDTO>
{
    public Guid LocationId { get; set; }
    public string? Address { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public Guid? UserId { get; set; }
}
