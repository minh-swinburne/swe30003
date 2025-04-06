namespace SmartRide.Application.DTOs.Vehicles;

public class DeleteVehicleRequestDTO : BaseRequestDTO
{
    public required Guid VehicleId { get; init; }
}
