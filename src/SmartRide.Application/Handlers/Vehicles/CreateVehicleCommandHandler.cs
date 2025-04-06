using AutoMapper;
using SmartRide.Application.Commands;
using SmartRide.Application.Commands.Vehicles;
using SmartRide.Application.DTOs.Vehicles;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Application.Handlers.Vehicles;

public class CreateVehicleCommandHandler(IRepository<Vehicle> vehicleRepository, IMapper mapper)
    : BaseCommandHandler<CreateVehicleCommand, CreateVehicleResponseDTO>
{
    private readonly IRepository<Vehicle> _vehicleRepository = vehicleRepository;
    private readonly IMapper _mapper = mapper;

    public override async Task<CreateVehicleResponseDTO> Handle(CreateVehicleCommand command, CancellationToken cancellationToken)
    {
        var vehicle = _mapper.Map<Vehicle>(command);
        var createdVehicle = await _vehicleRepository.CreateAsync(vehicle, cancellationToken);
        return _mapper.Map<CreateVehicleResponseDTO>(createdVehicle);
    }
}
