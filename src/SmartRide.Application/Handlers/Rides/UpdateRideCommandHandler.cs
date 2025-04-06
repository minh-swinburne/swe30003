using AutoMapper;
using SmartRide.Application.Commands.Rides;
using SmartRide.Application.DTOs.Rides;
using SmartRide.Common.Exceptions;
using SmartRide.Common.Responses.Errors;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Application.Handlers.Rides;

public class UpdateRideCommandHandler(IRepository<Ride> rideRepository, IMapper mapper)
    : BaseCommandHandler<UpdateRideCommand, UpdateRideResponseDTO>
{
    private readonly IRepository<Ride> _rideRepository = rideRepository;
    private readonly IMapper _mapper = mapper;

    public override async Task<UpdateRideResponseDTO> Handle(UpdateRideCommand command, CancellationToken cancellationToken)
    {
        var ride = await _rideRepository.GetByIdAsync(command.RideId, cancellationToken)
            ?? throw new BaseException(RideErrors.Module, RideErrors.ID_NOT_FOUND.FormatMessage(("RideId", command.RideId)));

        _mapper.Map(command, ride);
        var updatedRide = await _rideRepository.UpdateAsync(ride, cancellationToken);
        return _mapper.Map<UpdateRideResponseDTO>(updatedRide);
    }
}
