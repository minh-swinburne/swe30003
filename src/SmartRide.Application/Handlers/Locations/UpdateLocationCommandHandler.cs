using AutoMapper;
using SmartRide.Application.Commands.Locations;
using SmartRide.Application.DTOs.Locations;
using SmartRide.Common.Exceptions;
using SmartRide.Common.Responses.Errors;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Application.Handlers.Locations;

public class UpdateLocationCommandHandler(IRepository<Location> locationRepository, IMapper mapper)
    : BaseCommandHandler<UpdateLocationCommand, UpdateLocationResponseDTO>
{
    private readonly IRepository<Location> _locationRepository = locationRepository;
    private readonly IMapper _mapper = mapper;

    public override async Task<UpdateLocationResponseDTO> Handle(UpdateLocationCommand command, CancellationToken cancellationToken)
    {
        var location = await _locationRepository.GetByIdAsync(command.LocationId, cancellationToken)
            ?? throw new BaseException(LocationErrors.Module, LocationErrors.ID_NOT_FOUND.FormatMessage(("LocationId", command.LocationId)));

        _mapper.Map(command, location);

        var updatedLocation = await _locationRepository.UpdateAsync(location, cancellationToken);
        return _mapper.Map<UpdateLocationResponseDTO>(updatedLocation);
    }
}
