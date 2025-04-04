namespace SmartRide.Application.DTOs.Vehicles;

public class GetVehicleByIdRequestDTO : BaseDTO
{
    public required Guid VehicleId { get; init; }
}
