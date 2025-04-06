using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartRide.Application.DTOs.Vehicles;
using SmartRide.Application.Queries.Vehicles;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Application.Handlers.Vehicles;

public class GetVehicleByVinQueryHandler(IRepository<Vehicle> vehicleRepository, IMapper mapper)
    : BaseQueryHandler<GetVehicleByVinQuery, GetVehicleResponseDTO>
{
    private readonly IRepository<Vehicle> _vehicleRepository = vehicleRepository;
    private readonly IMapper _mapper = mapper;

    public override async Task<GetVehicleResponseDTO> Handle(GetVehicleByVinQuery query, CancellationToken cancellationToken)
    {
        var vehicle = await _vehicleRepository.Query(cancellationToken)
            .FirstOrDefaultAsync(v => v.Vin == query.Vin, cancellationToken);

        return _mapper.Map<GetVehicleResponseDTO>(vehicle);
    }
}
