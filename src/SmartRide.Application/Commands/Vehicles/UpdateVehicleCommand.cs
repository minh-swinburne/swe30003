using SmartRide.Application.DTOs.Vehicles;

namespace SmartRide.Application.Commands.Vehicles;

public class UpdateVehicleCommand : BaseCommand<UpdateVehicleResponseDTO>
{
    public required Guid VehicleId { get; init; }
    public string? Plate { get; init; }
    public string? Make { get; init; }
    public string? Model { get; init; }
    public int? Year { get; init; }
    public DateTime? RegisteredDate { get; init; }
}
