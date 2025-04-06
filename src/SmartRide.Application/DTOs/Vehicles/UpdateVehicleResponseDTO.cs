namespace SmartRide.Application.DTOs.Vehicles;

public class UpdateVehicleResponseDTO
{
    public required Guid VehicleId { get; init; }
    public required string Plate { get; init; }
    public required string Make { get; init; }
    public required string Model { get; init; }
    public required int Year { get; init; }
    public required DateTime RegisteredDate { get; init; }
}
