using AutoMapper;
using SmartRide.Application.Commands.Rides;
using SmartRide.Application.DTOs.Rides;
using SmartRide.Common.Exceptions;
using SmartRide.Common.Responses.Errors;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Application.Handlers.Rides;

public class CreateRideCommandHandler(IRepository<Ride> rideRepository, IRepository<User> userRepository, IMapper mapper)
    : BaseCommandHandler<CreateRideCommand, CreateRideResponseDTO>
{
    private readonly IRepository<Ride> _rideRepository = rideRepository;
    private readonly IRepository<User> _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;

    public override async Task<CreateRideResponseDTO> Handle(CreateRideCommand command, CancellationToken cancellationToken)
    {
        var passenger = await _userRepository.GetByIdAsync(
            command.PassengerId,
            [
                p => p.Roles,
                p => p.Rides,
            ],
            cancellationToken: cancellationToken
            ) ?? throw new BaseException(RideErrors.Module, RideErrors.PASSENGER_ID_NOT_FOUND.FormatMessage(("UserId", command.PassengerId)));

        if (passenger.ActiveRides().Any())
            throw new BaseException(RideErrors.Module, RideErrors.PASSENGER_ALREADY_IN_RIDE.FormatMessage(("UserId", command.PassengerId)));

        var ride = _mapper.Map<Ride>(command);
        var createdRide = await _rideRepository.CreateAsync(ride, cancellationToken);
        return _mapper.Map<CreateRideResponseDTO>(createdRide);
    }
}
