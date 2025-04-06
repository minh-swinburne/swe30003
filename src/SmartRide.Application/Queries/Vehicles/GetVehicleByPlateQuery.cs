using SmartRide.Application.DTOs.Vehicles;

namespace SmartRide.Application.Queries.Vehicles;

public class GetVehicleByPlateQuery : BaseQuery<GetVehicleResponseDTO>
{
    public string Plate { get; init; } = string.Empty;
}
