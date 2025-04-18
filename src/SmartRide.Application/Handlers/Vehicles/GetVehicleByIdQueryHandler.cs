using AutoMapper;
using SmartRide.Application.DTOs.Vehicles;
using SmartRide.Application.Queries.Vehicles;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Application.Handlers.Vehicles;

public class GetVehicleByIdQueryHandler(IRepository<Vehicle> vehicleRepository, IMapper mapper)
    : BaseQueryHandler<GetVehicleByIdQuery, GetVehicleResponseDTO>
{
    private readonly IRepository<Vehicle> _vehicleRepository = vehicleRepository;
    private readonly IMapper _mapper = mapper;

    public override async Task<GetVehicleResponseDTO> Handle(GetVehicleByIdQuery request, CancellationToken cancellationToken)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(
            request.VehicleId,
            [
                v => v.User,
                v => v.VehicleType,
            ],
            cancellationToken
            );
        return _mapper.Map<GetVehicleResponseDTO>(vehicle);
    }
}
