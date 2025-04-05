namespace SmartRide.Application.DTOs.Vehicles;

public class GetVehicleByIdRequestDTO : BaseRequestDTO
{
    public required Guid VehicleId { get; init; }
}
