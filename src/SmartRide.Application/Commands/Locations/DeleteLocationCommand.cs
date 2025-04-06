using SmartRide.Application.DTOs.Locations;

namespace SmartRide.Application.Commands.Locations;

public class DeleteLocationCommand : BaseCommand<DeleteLocationResponseDTO>
{
    public Guid LocationId { get; set; }
}
