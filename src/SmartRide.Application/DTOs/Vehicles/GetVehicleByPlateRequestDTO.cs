namespace SmartRide.Application.DTOs.Vehicles;

public class GetVehicleByPlateRequestDTO : BaseRequestDTO
{
    public required string Plate { get; init; }
}
