using SmartRide.Application.DTOs.Vehicles;

namespace SmartRide.Application.Commands.Vehicles;

public class CreateVehicleCommand : BaseCommand<CreateVehicleResponseDTO>
{
    public Guid UserId { get; init; }
    public byte VehicleTypeId { get; init; }
    public string Vin { get; init; } = string.Empty;
    public string Plate { get; init; } = string.Empty;
    public string Make { get; init; } = string.Empty;
    public string Model { get; init; } = string.Empty;
    public int Year { get; init; }
    public DateTime RegisteredDate { get; init; }
}
