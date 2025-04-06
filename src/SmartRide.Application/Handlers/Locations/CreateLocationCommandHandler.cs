using AutoMapper;
using SmartRide.Application.Commands.Locations;
using SmartRide.Application.DTOs.Locations;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Application.Handlers.Locations;

public class CreateLocationCommandHandler(IRepository<Location> locationRepository, IMapper mapper)
    : BaseCommandHandler<CreateLocationCommand, CreateLocationResponseDTO>
{
    private readonly IRepository<Location> _locationRepository = locationRepository;
    private readonly IMapper _mapper = mapper;

    public override async Task<CreateLocationResponseDTO> Handle(CreateLocationCommand command, CancellationToken cancellationToken)
    {
        var location = _mapper.Map<Location>(command);
        var createdLocation = await _locationRepository.CreateAsync(location, cancellationToken);
        return _mapper.Map<CreateLocationResponseDTO>(createdLocation);
    }
}
