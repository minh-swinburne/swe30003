using SmartRide.Application.DTOs.Rides;

namespace SmartRide.Application.Commands.Rides;

public class DeleteRideCommand : BaseCommand<DeleteRideResponseDTO>
{
    public Guid RideId { get; set; }
}
