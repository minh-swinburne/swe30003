using AutoMapper;
using SmartRide.Application.DTOs.Vehicles;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Application.Queries.Vehicles.Handlers;

public class GetVehicleByIdQueryHandler(IRepository<Vehicle> vehicleRepository, IMapper mapper)
    : BaseQueryHandler<GetVehicleByIdQuery, GetVehicleResponseDTO>
{
    private readonly IRepository<Vehicle> _vehicleRepository = vehicleRepository;
    private readonly IMapper _mapper = mapper;

    public override async Task<GetVehicleResponseDTO> Handle(GetVehicleByIdQuery request, CancellationToken cancellationToken)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(request.Id, cancellationToken);
        return _mapper.Map<GetVehicleResponseDTO>(vehicle);
    }
}
