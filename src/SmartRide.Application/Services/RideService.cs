using AutoMapper;
using MediatR;
using SmartRide.Application.Commands;
using SmartRide.Application.Commands.Locations;
using SmartRide.Application.Commands.Payments;
using SmartRide.Application.Commands.Rides;
using SmartRide.Application.DTOs;
using SmartRide.Application.DTOs.Locations;
using SmartRide.Application.DTOs.Rides;
using SmartRide.Application.Factories;
using SmartRide.Application.Interfaces;
using SmartRide.Application.Queries.Locations;
using SmartRide.Application.Queries.Rides;
using SmartRide.Common.Exceptions;
using SmartRide.Common.Responses.Errors;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Application.Services;

public class RideService(IMediator mediator, IMapper mapper, IMapService mapService, ILocationService locationService) : IRideService
{
    private readonly IMediator _mediator = mediator;
    private readonly IMapper _mapper = mapper;
    private readonly IMapService _mapService = mapService;
    private readonly ILocationService _locationService = locationService;

    public async Task<ListResponseDTO<ListRideResponseDTO>> ListRidesAsync(ListRideRequestDTO request)
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
        var pickupLocation = await _locationService.GetOrCreateLocationAsync(request.PickupAddress, request.PickupLatitude, request.PickupLongitude);
        var destinationLocation = await _locationService.GetOrCreateLocationAsync(request.DestinationAddress, request.DestinationLatitude, request.DestinationLongitude);

        // Step 2: Get distance and estimated time
        var (distance, _) = await _mapService.CalculateDistanceAndTimeAsync(
            pickupLocation.Latitude!.Value,
            pickupLocation.Longitude!.Value,
            destinationLocation.Latitude!.Value,
            destinationLocation.Longitude!.Value
        );

        var fare = CalculateFare(distance);

        // Step 3: Create the ride
        var rideCommand = new CreateRideCommand
        {
            PassengerId = request.PassengerId,
            RideType = request.RideType,
            PickupLocationId = pickupLocation.LocationId,
            DestinationId = destinationLocation.LocationId,
            // PickupETA = pickupETA,
            // ArrivalETA = pickupETA.AddMinutes(estimatedTime),
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

    private static decimal CalculateFare(double distanceInKm)
    {
        const decimal baseFare = 2.50m;
        const decimal perKmRate = 0.50m;
        return baseFare + (decimal)distanceInKm * perKmRate;
    }

    public async Task<ResponseDTO<UpdateRideResponseDTO>> UpdateRideAsync(UpdateRideRequestDTO request)
    {
        var command = MediatRFactory.CreateCommand<UpdateRideCommand>(request);
        var result = await _mediator.Send(command);
        await _mediator.Send(new SaveChangesCommand());
        return new ResponseDTO<UpdateRideResponseDTO> { Data = result };
    }

    public async Task<ResponseDTO<MatchRideResponseDTO>> MatchRideAsync(MatchRideRequestDTO request)
    {
        var command = MediatRFactory.CreateCommand<MatchRideCommand>(request);
        var result = await _mediator.Send(command);
        await _mediator.Send(new SaveChangesCommand());
        return new ResponseDTO<MatchRideResponseDTO> { Data = result };
    }

    public async Task<ResponseDTO<DeleteRideResponseDTO>> DeleteRideAsync(DeleteRideRequestDTO request)
    {
        var command = MediatRFactory.CreateCommand<DeleteRideCommand>(request);
        var result = await _mediator.Send(command);
        await _mediator.Send(new SaveChangesCommand());
        return new ResponseDTO<DeleteRideResponseDTO> { Data = result };
    }
}
