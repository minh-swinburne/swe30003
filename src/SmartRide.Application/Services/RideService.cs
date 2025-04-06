using MediatR;
using SmartRide.Application.Commands;
using SmartRide.Application.Commands.Rides;
using SmartRide.Application.Commands.Locations;
using SmartRide.Application.Commands.Payments;
using SmartRide.Application.DTOs;
using SmartRide.Application.DTOs.Rides;
using SmartRide.Application.Interfaces;
using SmartRide.Domain.Interfaces;
using SmartRide.Application.Factories;
using SmartRide.Application.Queries.Rides;
using SmartRide.Application.DTOs.Locations;
using SmartRide.Application.Queries.Locations;
using AutoMapper;

namespace SmartRide.Application.Services;

public class RideService(IMediator mediator, IMapper mapper, IMapService mapService) : IRideService
{
    private readonly IMediator _mediator = mediator;
    private readonly IMapper _mapper = mapper;
    private readonly IMapService _mapService = mapService;

    public async Task<ListResponseDTO<ListRideResponseDTO>> GetAllRidesAsync(ListRideRequestDTO request)
    {
        var query = MediatRFactory.CreateQuery<ListRideQuery>(request);
        var result = await _mediator.Send(query);
        return new ListResponseDTO<ListRideResponseDTO>
        {
            Data = result,
            Count = result.Count
        };
    }

    public async Task<ResponseDTO<GetRideResponseDTO>> GetRideByIdAsync(GetRideByIdRequestDTO request)
    {
        var query = MediatRFactory.CreateQuery<GetRideByIdQuery>(request);
        var result = await _mediator.Send(query);
        return new ResponseDTO<GetRideResponseDTO> { Data = result };
    }

    public async Task<ResponseDTO<CreateRideResponseDTO>> CreateRideAsync(CreateRideRequestDTO request)
    {
        // Step 1: Get or create locations for pickup and destination
        var pickupLocation = await GetOrCreateLocationAsync(request.PickupLocation);
        var destinationLocation = await GetOrCreateLocationAsync(request.Destination);

        // Step 2: Calculate distance, fare, and ETAs
        var distance = _mapService.CalculateDistance(
            pickupLocation.Latitude!.Value,
            pickupLocation.Longitude!.Value,
            destinationLocation.Latitude!.Value,
            destinationLocation.Longitude!.Value
        );
        var fare = _mapService.CalculateFare(distance);
        var pickupETA = DateTime.UtcNow.AddMinutes(_mapService.EstimatePickupTime(distance));
        var arrivalETA = pickupETA.AddMinutes(_mapService.EstimateTravelTime(distance));

        // Step 3: Create the ride
        var rideCommand = new CreateRideCommand
        {
            PassengerId = request.PassengerId,
            RideType = request.RideType,
            PickupLocationId = pickupLocation.LocationId,
            DestinationId = destinationLocation.LocationId,
            PickupETA = pickupETA,
            ArrivalETA = arrivalETA,
            Fare = fare,
            Notes = request.Notes
        };
        var rideResult = await _mediator.Send(rideCommand);

        // Step 4: Create a pending payment for the ride
        var paymentCommand = new CreatePaymentCommand
        {
            RideId = rideResult.RideId,
            Amount = fare,
            PaymentMethodId = request.PaymentMethod
        };
        await _mediator.Send(paymentCommand);

        // Step 5: Save changes
        await _mediator.Send(new SaveChangesCommand());

        return new ResponseDTO<CreateRideResponseDTO> { Data = rideResult };
    }

    public async Task<ResponseDTO<UpdateRideResponseDTO>> UpdateRideAsync(UpdateRideRequestDTO request)
    {
        var command = MediatRFactory.CreateCommand<UpdateRideCommand>(request);
        var result = await _mediator.Send(command);
        await _mediator.Send(new SaveChangesCommand());
        return new ResponseDTO<UpdateRideResponseDTO> { Data = result };
    }

    public async Task<ResponseDTO<DeleteRideResponseDTO>> DeleteRideAsync(DeleteRideRequestDTO request)
    {
        var command = MediatRFactory.CreateCommand<DeleteRideCommand>(request);
        var result = await _mediator.Send(command);
        await _mediator.Send(new SaveChangesCommand());
        return new ResponseDTO<DeleteRideResponseDTO> { Data = result };
    }

    private async Task<GetLocationResponseDTO> GetOrCreateLocationAsync(string address)
    {
        // Check if the location already exists
        var existingLocations = await _mediator.Send(new ListLocationQuery { Address = address });
        if (existingLocations.Count != 0)
        {
            var existingLocation = existingLocations.First();
            return _mapper.Map<GetLocationResponseDTO>(existingLocation);
        }

        // If not, create a new location using the map service
        var (Latitude, Longitude) = await _mapService.GetCoordinatesAsync(address);
        var createLocationCommand = new CreateLocationCommand
        {
            Address = address,
            Latitude = Latitude,
            Longitude = Longitude
        };

        var newLocation = await _mediator.Send(createLocationCommand);
        return _mapper.Map<GetLocationResponseDTO>(newLocation);
    }
}
