using SmartRide.Application.DTOs.Lookup;

namespace SmartRide.Application.DTOs.Vehicles;

public class ListVehicleResponseDTO
{
    public required Guid VehicleId { get; init; }
    public required VehicleTypeDTO VehicleType { get; init; }
    public required string Vin { get; init; }
    public required string Plate { get; init; }
    public required string Make { get; init; }
    public required string Model { get; init; }
    public int Year { get; init; }
    public DateTime RegisteredDate { get; init; }
}
