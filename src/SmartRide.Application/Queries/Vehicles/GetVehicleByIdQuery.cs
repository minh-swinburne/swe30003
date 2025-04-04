using SmartRide.Application.DTOs.Vehicles;

namespace SmartRide.Application.Queries.Vehicles;

public class GetVehicleByIdQuery : BaseQuery<GetVehicleByIdResponseDTO>
{
    public Guid Id { get; init; }
}
