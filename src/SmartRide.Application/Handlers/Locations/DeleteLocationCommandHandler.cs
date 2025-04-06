using SmartRide.Application.Commands.Locations;
using SmartRide.Application.DTOs.Locations;
using SmartRide.Common.Exceptions;
using SmartRide.Common.Responses.Errors;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Application.Handlers.Locations;

public class DeleteLocationCommandHandler(IRepository<Location> locationRepository)
    : BaseCommandHandler<DeleteLocationCommand, DeleteLocationResponseDTO>
{
    private readonly IRepository<Location> _locationRepository = locationRepository;

    public override async Task<DeleteLocationResponseDTO> Handle(DeleteLocationCommand command, CancellationToken cancellationToken)
    {
        var location = await _locationRepository.GetByIdAsync(command.LocationId, cancellationToken)
            ?? throw new BaseException(LocationErrors.Module, LocationErrors.ID_NOT_FOUND.FormatMessage(("LocationId", command.LocationId)));

        await _locationRepository.DeleteAsync(location.Id, cancellationToken);

        return new DeleteLocationResponseDTO
        {
            LocationId = location.Id,
            Success = true,
        };
    }
}
