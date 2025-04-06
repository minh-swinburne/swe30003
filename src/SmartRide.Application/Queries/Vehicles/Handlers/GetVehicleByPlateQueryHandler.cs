using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartRide.Application.DTOs.Vehicles;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Application.Queries.Vehicles.Handlers;

public class GetVehicleByPlateQueryHandler(IRepository<Vehicle> vehicleRepository, IMapper mapper)
    : BaseQueryHandler<GetVehicleByPlateQuery, GetVehicleResponseDTO>
{
    private readonly IRepository<Vehicle> _vehicleRepository = vehicleRepository;
    private readonly IMapper _mapper = mapper;

    public override async Task<GetVehicleResponseDTO> Handle(GetVehicleByPlateQuery query, CancellationToken cancellationToken)
    {
        var vehicle = await _vehicleRepository.Query(cancellationToken)
            .FirstOrDefaultAsync(v => v.Plate == query.Plate, cancellationToken);

        return _mapper.Map<GetVehicleResponseDTO>(vehicle);
    }
}
