namespace SmartRide.Application.DTOs.Vehicles;

public class DeleteVehicleResponseDTO
{
    public required Guid VehicleId { get; init; }
    public required bool Success { get; init; }
}
