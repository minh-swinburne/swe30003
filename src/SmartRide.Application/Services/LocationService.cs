using MediatR;
using AutoMapper;
using SmartRide.Application.Commands;
using SmartRide.Application.Commands.Locations;
using SmartRide.Application.DTOs;
using SmartRide.Application.DTOs.Locations;
using SmartRide.Application.Factories;
using SmartRide.Application.Interfaces;
using SmartRide.Application.Queries.Locations;
using SmartRide.Domain.Interfaces;
using SmartRide.Common.Exceptions;
using SmartRide.Common.Responses.Errors;

namespace SmartRide.Application.Services;

public class LocationService(IMediator mediator, IMapper mapper, IMapService mapService) : ILocationService
{
    private readonly IMediator _mediator = mediator;
    private readonly IMapper _mapper = mapper;
    private readonly IMapService _mapService = mapService;

    public async Task<ListResponseDTO<ListLocationResponseDTO>> ListLocationsAsync(ListLocationRequestDTO request)
    {
        var query = MediatRFactory.CreateQuery<ListLocationQuery>(request);
        var result = await _mediator.Send(query);
        return new ListResponseDTO<ListLocationResponseDTO>
        {
            Data = result,
            Count = result.Count
        };
    }

    public async Task<ResponseDTO<GetLocationResponseDTO>> GetLocationByIdAsync(GetLocationByIdRequestDTO request)
    {
        var query = MediatRFactory.CreateQuery<GetLocationByIdQuery>(request);
        var result = await _mediator.Send(query);
        return new ResponseDTO<GetLocationResponseDTO> { Data = result };
    }

    public async Task<ResponseDTO<CreateLocationResponseDTO>> CreateLocationAsync(CreateLocationRequestDTO request)
    {
        var command = MediatRFactory.CreateCommand<CreateLocationCommand>(request);
        var result = await _mediator.Send(command);
        await _mediator.Send(new SaveChangesCommand());
        return new ResponseDTO<CreateLocationResponseDTO> { Data = result };
    }

    public async Task<ResponseDTO<UpdateLocationResponseDTO>> UpdateLocationAsync(UpdateLocationRequestDTO request)
    {
        var command = MediatRFactory.CreateCommand<UpdateLocationCommand>(request);
        var result = await _mediator.Send(command);
        await _mediator.Send(new SaveChangesCommand());
        return new ResponseDTO<UpdateLocationResponseDTO> { Data = result };
    }

    public async Task<ResponseDTO<DeleteLocationResponseDTO>> DeleteLocationAsync(DeleteLocationRequestDTO request)
    {
        var command = MediatRFactory.CreateCommand<DeleteLocationCommand>(request);
        var result = await _mediator.Send(command);
        await _mediator.Send(new SaveChangesCommand());
        return new ResponseDTO<DeleteLocationResponseDTO> { Data = result };
    }

    public async Task<GetLocationResponseDTO> GetOrCreateLocationAsync(string? address, double? latitude, double? longitude)
    {
        // If coordinates are provided, skip geocoding
        if (latitude.HasValue && longitude.HasValue)
        {
            var existingLocation = await _mediator.Send(new ListLocationQuery
            {
                LatitudeFrom = latitude,
                LatitudeTo = latitude,
                LongitudeFrom = longitude,
                LongitudeTo = longitude
            });

            if (existingLocation.Count != 0)
            {
                return _mapper.Map<GetLocationResponseDTO>(existingLocation.First());
            }

            // Create a new location with coordinates
            var createLocationCommand = new CreateLocationCommand
            {
                Address = address ?? await _mapService.GetAddressAsync(latitude.Value, longitude.Value),
                Latitude = latitude.Value,
                Longitude = longitude.Value
            };
            var newLocation = await _mediator.Send(createLocationCommand);
            return _mapper.Map<GetLocationResponseDTO>(newLocation);
        }

        // If only address is provided, geocode it
        if (!string.IsNullOrWhiteSpace(address))
        {
            var existingLocation = await _mediator.Send(new ListLocationQuery { Address = address });
            if (existingLocation.Count != 0)
            {
                return _mapper.Map<GetLocationResponseDTO>(existingLocation.First());
            }

            (latitude, longitude) = await _mapService.GetCoordinatesAsync(address);
            var createLocationCommand = new CreateLocationCommand
            {
                Address = address,
                Latitude = latitude,
                Longitude = longitude
            };
            var newLocation = await _mediator.Send(createLocationCommand);
            return _mapper.Map<GetLocationResponseDTO>(newLocation);
        }

        throw new BaseException(LocationErrors.Module, LocationErrors.CREATE_REQUEST_INVALID);
    }
}
