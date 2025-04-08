using AutoMapper;
using SmartRide.Application.DTOs.Rides;
using SmartRide.Application.Interfaces;
using SmartRide.Application.Queries.Rides;
using SmartRide.Common.Extensions;
using SmartRide.Common.Helpers;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;
using System.Linq.Expressions;

namespace SmartRide.Application.Handlers.Rides;

public class ListRideQueryHandler(IRepository<Ride> rideRepository, IMapper mapper)
    : BaseQueryHandler<ListRideQuery, List<ListRideResponseDTO>>, IFilterableHandler<ListRideQuery, Ride>
{
    private readonly IRepository<Ride> _rideRepository = rideRepository;
    private readonly IMapper _mapper = mapper;

    public override async Task<List<ListRideResponseDTO>> Handle(ListRideQuery query, CancellationToken cancellationToken)
    {
        Expression<Func<Ride, object>>? orderBy = null;
        Expression<Func<Ride, bool>>? filter = BuildFilter(query);

        if (!string.IsNullOrEmpty(query.OrderBy))
        {
            var propertyInfo = QueryHelper.GetProperty<Ride>(query.OrderBy);
            orderBy = QueryHelper.GetSortExpression<Ride>(propertyInfo!);
        }

        var rides = await _rideRepository.GetWithFilterAsync<ListRideResponseDTO>(
            filter,
            orderBy: orderBy,
            ascending: query.Ascending,
            skip: query.PageSize * (query.PageNo - 1),
            limit: query.PageSize,
            includes: ["VehicleType"],
            cancellationToken: cancellationToken
        );

        return _mapper.Map<List<ListRideResponseDTO>>(rides);
    }

    public Expression<Func<Ride, bool>>? BuildFilter(ListRideQuery query)
    {
        Expression<Func<Ride, bool>>? filter = null;

        if (query.PassengerId.HasValue)
            filter = filter.AddFilter(ride => ride.PassengerId == query.PassengerId.Value);

        if (query.DriverId.HasValue)
            filter = filter.AddFilter(ride => ride.DriverId == query.DriverId.Value);

        if (query.VehicleId.HasValue)
            filter = filter.AddFilter(ride => ride.VehicleId == query.VehicleId.Value);

        if (query.VehicleType.HasValue)
            filter = filter.AddFilter(ride => ride.VehicleTypeId == query.VehicleType.Value);

        if (query.RideType.HasValue)
            filter = filter.AddFilter(ride => ride.RideType == query.RideType.Value);

        if (query.RideStatus.HasValue)
            filter = filter.AddFilter(ride => ride.Status == query.RideStatus.Value);

        if (query.PickupDateFrom.HasValue)
            filter = filter.AddFilter(ride => ride.PickupETA >= query.PickupDateFrom.Value);

        if (query.PickupDateTo.HasValue)
            filter = filter.AddFilter(ride => ride.PickupETA <= query.PickupDateTo.Value);

        return filter;
    }
}
