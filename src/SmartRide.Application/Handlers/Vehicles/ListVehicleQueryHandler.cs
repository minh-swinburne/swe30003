using AutoMapper;
using Humanizer;
using SmartRide.Application.DTOs.Vehicles;
using SmartRide.Application.Interfaces;
using SmartRide.Application.Queries.Vehicles;
using SmartRide.Common.Extensions;
using SmartRide.Common.Helpers;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;
using System.Linq.Expressions;
using System.Reflection;

namespace SmartRide.Application.Handlers.Vehicles;

public class ListVehicleQueryHandler(IRepository<Vehicle> vehicleRepository, IMapper mapper)
    : BaseQueryHandler<ListVehicleQuery, List<ListVehicleResponseDTO>>, IFilterableHandler<ListVehicleQuery, Vehicle>
{
    private readonly IRepository<Vehicle> _vehicleRepository = vehicleRepository;
    private readonly IMapper _mapper = mapper;

    public override async Task<List<ListVehicleResponseDTO>> Handle(ListVehicleQuery query, CancellationToken cancellationToken)
    {
        Expression<Func<Vehicle, object>>? orderBy = null;
        Expression<Func<Vehicle, bool>>? filter = BuildFilter(query);

        if (!string.IsNullOrEmpty(query.OrderBy))
        {
            var propertyInfo = QueryHelper.GetProperty<Vehicle>(query.OrderBy);
            orderBy = QueryHelper.GetSortExpression<Vehicle>(propertyInfo!);
        }

        List<Vehicle> result = await _vehicleRepository.GetWithFilterAsync<ListVehicleResponseDTO>(
            filter,
            orderBy: orderBy,
            ascending: query.Ascending,
            skip: query.PageSize * (query.PageNo - 1),
            limit: query.PageSize,
            cancellationToken: cancellationToken
        );

        return _mapper.Map<List<ListVehicleResponseDTO>>(result);
    }

    public Expression<Func<Vehicle, bool>>? BuildFilter(ListVehicleQuery query)
    {
        Expression<Func<Vehicle, bool>>? filter = null;

        if (!string.IsNullOrWhiteSpace(query.Make))
            filter = filter.AddFilter(vehicle => vehicle.Make.Contains(query.Make));

        if (!string.IsNullOrWhiteSpace(query.Model))
            filter = filter.AddFilter(vehicle => vehicle.Model.Contains(query.Model));

        if (query.Year.HasValue)
            filter = filter.AddFilter(vehicle => vehicle.Year == query.Year);

        if (query.RegisteredDateFrom.HasValue)
            filter = filter.AddFilter(vehicle => vehicle.RegisteredDate >= query.RegisteredDateFrom.Value);

        if (query.RegisteredDateTo.HasValue)
            filter = filter.AddFilter(vehicle => vehicle.RegisteredDate <= query.RegisteredDateTo.Value);

        return filter;
    }
}
