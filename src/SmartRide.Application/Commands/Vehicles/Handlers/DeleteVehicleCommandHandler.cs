using SmartRide.Application.DTOs.Vehicles;
using SmartRide.Common.Exceptions;
using SmartRide.Common.Responses.Errors;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Application.Commands.Vehicles.Handlers;

public class DeleteVehicleCommandHandler(IRepository<Vehicle> vehicleRepository)
    : BaseCommandHandler<DeleteVehicleCommand, DeleteVehicleResponseDTO>
{
    private readonly IRepository<Vehicle> _vehicleRepository = vehicleRepository;

    public override async Task<DeleteVehicleResponseDTO> Handle(DeleteVehicleCommand command, CancellationToken cancellationToken)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(command.VehicleId, cancellationToken)
            ?? throw new BaseException(VehicleErrors.Module, VehicleErrors.ID_NOT_FOUND.FormatMessage(("VehicleId", command.VehicleId)));

        await _vehicleRepository.DeleteAsync(vehicle.Id, cancellationToken);

        return new DeleteVehicleResponseDTO
        {
            VehicleId = vehicle.Id,
            Success = true,
        };
    }
}
