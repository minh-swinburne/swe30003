using SmartRide.Application.Commands.Rides;
using SmartRide.Application.DTOs.Rides;
using SmartRide.Common.Exceptions;
using SmartRide.Common.Responses.Errors;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Application.Handlers.Rides;

public class DeleteRideCommandHandler(IRepository<Ride> rideRepository)
    : BaseCommandHandler<DeleteRideCommand, DeleteRideResponseDTO>
{
    private readonly IRepository<Ride> _rideRepository = rideRepository;

    public override async Task<DeleteRideResponseDTO> Handle(DeleteRideCommand command, CancellationToken cancellationToken)
    {
        var ride = await _rideRepository.GetByIdAsync(command.RideId, cancellationToken: cancellationToken)
            ?? throw new BaseException(RideErrors.Module, RideErrors.ID_NOT_FOUND.FormatMessage(("RideId", command.RideId)));

        await _rideRepository.DeleteAsync(command.RideId, cancellationToken);
        return new DeleteRideResponseDTO
        {
            RideId = ride.Id,
            Success = true
        };
    }
}
