using SmartRide.Application.DTOs.Lookup;

namespace SmartRide.Application.DTOs.Vehicles;

public class ListVehicleResponseDTO : BaseVehicleResponseDTO
{
    public required VehicleTypeDTO VehicleType { get; init; }
    public required string Vin { get; init; }
    public required string Plate { get; init; }
    public required string Make { get; init; }
    public required string Model { get; init; }
    public required int Year { get; init; }
    public required DateTime RegisteredDate { get; init; }
}
