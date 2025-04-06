using SmartRide.Application.DTOs.Vehicles;

namespace SmartRide.Application.Queries.Vehicles;

public class GetVehicleByIdQuery : BaseQuery<GetVehicleResponseDTO>
{
    public Guid Id { get; init; }
}
