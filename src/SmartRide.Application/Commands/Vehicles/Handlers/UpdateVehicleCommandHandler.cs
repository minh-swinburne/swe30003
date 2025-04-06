using AutoMapper;
using SmartRide.Application.DTOs.Vehicles;
using SmartRide.Common.Exceptions;
using SmartRide.Common.Responses.Errors;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Application.Commands.Vehicles.Handlers;

public class UpdateVehicleCommandHandler(IRepository<Vehicle> vehicleRepository, IMapper mapper)
    : BaseCommandHandler<UpdateVehicleCommand, UpdateVehicleResponseDTO>
{
    private readonly IRepository<Vehicle> _vehicleRepository = vehicleRepository;
    private readonly IMapper _mapper = mapper;

    public override async Task<UpdateVehicleResponseDTO> Handle(UpdateVehicleCommand command, CancellationToken cancellationToken)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(command.VehicleId, cancellationToken)
            ?? throw new BaseException(VehicleErrors.Module, VehicleErrors.ID_NOT_FOUND.FormatMessage(("VehicleId", command.VehicleId)));

        if (!string.IsNullOrWhiteSpace(command.Plate)) vehicle.Plate = command.Plate;
        if (!string.IsNullOrWhiteSpace(command.Make)) vehicle.Make = command.Make;
        if (!string.IsNullOrWhiteSpace(command.Model)) vehicle.Model = command.Model;
        if (command.Year.HasValue) vehicle.Year = command.Year.Value;
        if (command.RegisteredDate.HasValue) vehicle.RegisteredDate = command.RegisteredDate.Value;

        var updatedVehicle = await _vehicleRepository.UpdateAsync(vehicle, cancellationToken);
        return _mapper.Map<UpdateVehicleResponseDTO>(updatedVehicle);
    }
}
