using SmartRide.Application.DTOs.Vehicles;

namespace SmartRide.Application.Queries.Vehicles;

public class GetVehicleByPlateQuery : BaseQuery<GetVehicleResponseDTO>
{
    public required string Plate { get; init; }
}
