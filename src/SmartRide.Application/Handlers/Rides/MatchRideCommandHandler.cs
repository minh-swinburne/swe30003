using AutoMapper;
using SmartRide.Application.Commands.Rides;
using SmartRide.Application.DTOs.Rides;
using SmartRide.Common.Exceptions;
using SmartRide.Common.Responses.Errors;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Enums;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Application.Handlers.Rides;

public class MatchRideCommandHandler(IRepository<Ride> rideRepository, IRepository<User> userRepository, IRepository<Vehicle> vehicleRepository, IMapper mapper)
    : BaseCommandHandler<MatchRideCommand, MatchRideResponseDTO>
{
    private readonly IRepository<Ride> _rideRepository = rideRepository;
    private readonly IRepository<User> _userRepository = userRepository;
    private readonly IRepository<Vehicle> _vehicleRepository = vehicleRepository;
    private readonly IMapper _mapper = mapper;

    public override async Task<MatchRideResponseDTO> Handle(MatchRideCommand command, CancellationToken cancellationToken)
    {
        var ride = await _rideRepository.GetByIdAsync(command.RideId, cancellationToken: cancellationToken)
            ?? throw new BaseException(RideErrors.Module, RideErrors.ID_NOT_FOUND.FormatMessage(("RideId", command.RideId)));

        var driver = await _userRepository.GetByIdAsync(
            command.DriverId,
            [d => d.Rides],
            cancellationToken: cancellationToken
            ) ?? throw new BaseException(RideErrors.Module, RideErrors.DRIVER_ID_NOT_FOUND.FormatMessage(("UserId", command.DriverId)));

        var vehicle = await _vehicleRepository.GetByIdAsync(command.VehicleId, cancellationToken: cancellationToken)
            ?? throw new BaseException(RideErrors.Module, RideErrors.VEHICLE_ID_NOT_FOUND.FormatMessage(("VehicleId", command.VehicleId)));

        if (ride.RideType == RideTypeEnum.Shared && driver.ActiveRides().Count() > 3)
            throw new BaseException(RideErrors.Module, RideErrors.DRIVER_ALREADY_IN_RIDES.FormatMessage(("DriverId", command.DriverId)));
        else if (ride.RideType == RideTypeEnum.Standard && driver.ActiveRides().Any())
            throw new BaseException(RideErrors.Module, RideErrors.DRIVER_ALREADY_IN_RIDES.FormatMessage(("DriverId", command.VehicleId)));

        _mapper.Map(command, ride);
        ride.Status = RideStatusEnum.Picking;

        var updatedRide = await _rideRepository.UpdateAsync(ride, cancellationToken);

        return _mapper.Map<MatchRideResponseDTO>(updatedRide);
    }
}
