namespace SmartRide.Application.DTOs.Vehicles;

public class GetVehicleByVinRequestDTO : BaseRequestDTO
{
    public required string Vin { get; init; }
}
