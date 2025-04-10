using AutoMapper;
using SmartRide.Application.DTOs.Rides;
using SmartRide.Application.Queries.Rides;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Application.Handlers.Rides;

public class GetRideByIdQueryHandler(IRepository<Ride> rideRepository, IMapper mapper)
    : BaseQueryHandler<GetRideByIdQuery, GetRideResponseDTO>
{
    private readonly IRepository<Ride> _rideRepository = rideRepository;
    private readonly IMapper _mapper = mapper;

    public override async Task<GetRideResponseDTO> Handle(GetRideByIdQuery query, CancellationToken cancellationToken)
    {
        var ride = await _rideRepository.GetByIdAsync(
            query.RideId,
            [
                r => r.Passenger,
                r => r.Payment,
                r => r.Driver!,
                r => r.Vehicle!.VehicleType,
                r => r.VehicleType,
                r => r.PickupLocation,
                r => r.Destination,
                r => r.Payment!.PaymentMethod,
            ],
            cancellationToken) ?? throw new KeyNotFoundException($"Ride with ID {query.RideId} not found.");

        return _mapper.Map<GetRideResponseDTO>(ride);
    }
}
