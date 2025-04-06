using SmartRide.Application.DTOs.Vehicles;

namespace SmartRide.Application.Queries.Vehicles;

public class GetVehicleByVinQuery : BaseQuery<GetVehicleResponseDTO>
{
    public string Vin { get; init; } = string.Empty;
}
