using SmartRide.Application.DTOs.Vehicles;

namespace SmartRide.Application.Queries.Vehicles;

public class GetVehicleByVinQuery : BaseQuery<GetVehicleResponseDTO>
{
    public required string Vin { get; init; }
}
