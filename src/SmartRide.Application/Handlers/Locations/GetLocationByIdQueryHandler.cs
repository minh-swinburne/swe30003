using AutoMapper;
using SmartRide.Application.DTOs.Locations;
using SmartRide.Application.Queries.Locations;
using SmartRide.Common.Exceptions;
using SmartRide.Common.Responses.Errors;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Application.Handlers.Locations;

public class GetLocationByIdQueryHandler(IRepository<Location> locationRepository, IMapper mapper)
    : BaseQueryHandler<GetLocationByIdQuery, GetLocationResponseDTO>
{
    private readonly IRepository<Location> _locationRepository = locationRepository;
    private readonly IMapper _mapper = mapper;

    public override async Task<GetLocationResponseDTO> Handle(GetLocationByIdQuery query, CancellationToken cancellationToken)
    {
        var location = await _locationRepository.GetByIdAsync(query.LocationId, cancellationToken)
            ?? throw new BaseException(LocationErrors.Module, LocationErrors.ID_NOT_FOUND.FormatMessage(("LocationId", query.LocationId)));

        return _mapper.Map<GetLocationResponseDTO>(location);
    }
}
