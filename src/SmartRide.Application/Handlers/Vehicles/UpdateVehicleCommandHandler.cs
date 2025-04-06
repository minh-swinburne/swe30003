using AutoMapper;
using SmartRide.Application.Commands.Vehicles;
using SmartRide.Application.DTOs.Vehicles;
using SmartRide.Common.Exceptions;
using SmartRide.Common.Responses.Errors;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Application.Handlers.Vehicles;

public class UpdateVehicleCommandHandler(IRepository<Vehicle> vehicleRepository, IMapper mapper)
    : BaseCommandHandler<UpdateVehicleCommand, UpdateVehicleResponseDTO>
{
    private readonly IRepository<Vehicle> _vehicleRepository = vehicleRepository;
    private readonly IMapper _mapper = mapper;

    public override async Task<UpdateVehicleResponseDTO> Handle(UpdateVehicleCommand command, CancellationToken cancellationToken)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(command.VehicleId, cancellationToken)
            ?? throw new BaseException(VehicleErrors.Module, VehicleErrors.ID_NOT_FOUND.FormatMessage(("VehicleId", command.VehicleId)));

        _mapper.Map(command, vehicle);

        var updatedVehicle = await _vehicleRepository.UpdateAsync(vehicle, cancellationToken);
        return _mapper.Map<UpdateVehicleResponseDTO>(updatedVehicle);
    }
}
