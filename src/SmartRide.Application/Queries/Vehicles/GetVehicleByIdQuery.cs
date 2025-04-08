using SmartRide.Application.DTOs.Vehicles;

namespace SmartRide.Application.Queries.Vehicles;

public class GetVehicleByIdQuery : BaseQuery<GetVehicleResponseDTO>
{
    public Guid VehicleId { get; init; }
}
