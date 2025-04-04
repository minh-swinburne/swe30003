using AutoMapper;
using Humanizer;
using SmartRide.Application.DTOs.Vehicles;
using SmartRide.Application.Interfaces;
using SmartRide.Common.Extensions;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;
using System.Linq.Expressions;
using System.Reflection;

namespace SmartRide.Application.Queries.Vehicles.Handlers;

public class ListVehicleQueryHandler(IRepository<Vehicle> vehicleRepository, IMapper mapper)
    : BaseQueryHandler<ListVehicleQuery, List<ListVehicleResponseDTO>>, IFilterableHandler<ListVehicleQuery, Vehicle>
{
    private readonly IRepository<Vehicle> _vehicleRepository = vehicleRepository;
    private readonly IMapper _mapper = mapper;

    public override async Task<List<ListVehicleResponseDTO>> Handle(ListVehicleQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<Vehicle, object>>? orderBy = null;
        Expression<Func<Vehicle, bool>>? filter = BuildFilter(request);

        if (!string.IsNullOrEmpty(request.OrderBy))
        {
            var propertyInfo = typeof(Vehicle).GetProperty(request.OrderBy.Pascalize(), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                ?? throw new InvalidFilterCriteriaException($"Property {request.OrderBy} does not exist in {nameof(Vehicle)}.");

            var parameter = Expression.Parameter(typeof(Vehicle), "v");
            var propertyAccess = Expression.Property(parameter, propertyInfo);
            orderBy = (Expression<Func<Vehicle, object>>)Expression.Lambda(propertyAccess, parameter);
        }

        List<Vehicle> result = await _vehicleRepository.GetWithFilterAsync<ListVehicleResponseDTO>(
            filter,
            orderBy: orderBy,
            ascending: request.Ascending,
            skip: request.PageSize * (request.PageNo - 1),
            limit: request.PageSize,
            cancellationToken: cancellationToken
        );

        return _mapper.Map<List<ListVehicleResponseDTO>>(result);
    }

    public Expression<Func<Vehicle, bool>>? BuildFilter(ListVehicleQuery query)
    {
        Expression<Func<Vehicle, bool>>? filter = null;

        if (!string.IsNullOrWhiteSpace(query.Make))
            filter = filter.AddFilter(vehicle => vehicle.Make.Contains(query.Make, StringComparison.CurrentCultureIgnoreCase));

        if (!string.IsNullOrWhiteSpace(query.Model))
            filter = filter.AddFilter(vehicle => vehicle.Model.Contains(query.Model, StringComparison.CurrentCultureIgnoreCase));

        if (query.Year.HasValue)
            filter = filter.AddFilter(vehicle => vehicle.Year == query.Year);

        if (!string.IsNullOrWhiteSpace(query.Vin))
            filter = filter.AddFilter(vehicle => vehicle.Vin.Contains(query.Vin, StringComparison.CurrentCultureIgnoreCase));

        if (!string.IsNullOrWhiteSpace(query.Plate))
            filter = filter.AddFilter(vehicle => vehicle.Plate.Contains(query.Plate, StringComparison.CurrentCultureIgnoreCase));

        if (query.RegisteredDateFrom.HasValue)
            filter = filter.AddFilter(vehicle => vehicle.RegisteredDate >= query.RegisteredDateFrom.Value);

        if (query.RegisteredDateTo.HasValue)
            filter = filter.AddFilter(vehicle => vehicle.RegisteredDate <= query.RegisteredDateTo.Value);

        if (query.VehicleTypes is { Count: > 0 })
        {
            if (query.MatchAllVehicleTypes)
                filter = filter.AddFilter(vehicle => query.VehicleTypes.All(type => vehicle.VehicleTypeId == type));
            else
                filter = filter.AddFilter(vehicle => query.VehicleTypes.Contains(vehicle.VehicleTypeId));
        }

        return filter;
    }
}
