using AutoMapper;
using SmartRide.Application.Commands;
using SmartRide.Application.Commands.Rides;
using SmartRide.Application.DTOs.Rides;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Application.Handlers.Rides;

public class CreateRideCommandHandler(IRepository<Ride> rideRepository, IMapper mapper)
    : BaseCommandHandler<CreateRideCommand, CreateRideResponseDTO>
{
    private readonly IRepository<Ride> _rideRepository = rideRepository;
    private readonly IMapper _mapper = mapper;

    public override async Task<CreateRideResponseDTO> Handle(CreateRideCommand command, CancellationToken cancellationToken)
    {
        var ride = _mapper.Map<Ride>(command);
        var createdRide = await _rideRepository.CreateAsync(ride, cancellationToken);
        return _mapper.Map<CreateRideResponseDTO>(createdRide);
    }
}
