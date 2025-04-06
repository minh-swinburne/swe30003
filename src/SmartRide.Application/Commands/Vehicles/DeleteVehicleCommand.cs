using SmartRide.Application.DTOs.Vehicles;

namespace SmartRide.Application.Commands.Vehicles;

public class DeleteVehicleCommand : BaseCommand<DeleteVehicleResponseDTO>
{
    public required Guid VehicleId { get; init; }
}
