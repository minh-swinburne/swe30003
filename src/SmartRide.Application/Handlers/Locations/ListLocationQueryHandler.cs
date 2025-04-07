using AutoMapper;
using SmartRide.Application.DTOs.Locations;
using SmartRide.Application.Interfaces;
using SmartRide.Application.Queries.Locations;
using SmartRide.Common.Extensions;
using SmartRide.Common.Helpers;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;
using System.Linq.Expressions;

namespace SmartRide.Application.Handlers.Locations;

public class ListLocationQueryHandler(IRepository<Location> locationRepository, IMapper mapper)
    : BaseQueryHandler<ListLocationQuery, List<ListLocationResponseDTO>>, IFilterableHandler<ListLocationQuery, Location>
{
    private readonly IRepository<Location> _locationRepository = locationRepository;
    private readonly IMapper _mapper = mapper;

    public override async Task<List<ListLocationResponseDTO>> Handle(ListLocationQuery query, CancellationToken cancellationToken)
    {
        Expression<Func<Location, object>>? orderBy = null;
        Expression<Func<Location, bool>>? filter = BuildFilter(query);

        if (!string.IsNullOrEmpty(query.OrderBy))
        {
            var propertyInfo = QueryHelper.GetProperty<Location>(query.OrderBy);
            orderBy = QueryHelper.GetSortExpression<Location>(propertyInfo!);
        }

        var locations = await _locationRepository.GetWithFilterAsync<ListLocationResponseDTO>(
            filter,
            orderBy: orderBy,
            ascending: query.Ascending,
            skip: query.PageSize * (query.PageNo - 1),
            limit: query.PageSize,
            cancellationToken: cancellationToken
        );

        return _mapper.Map<List<ListLocationResponseDTO>>(locations);
    }

    public Expression<Func<Location, bool>>? BuildFilter(ListLocationQuery query)
    {
        Expression<Func<Location, bool>>? filter = null;

        if (query.UserId.HasValue)
            filter = filter.AddFilter(location => location.UserId == query.UserId.Value);

        if (!string.IsNullOrWhiteSpace(query.Address))
            filter = filter.AddFilter(location => location.Address.Contains(query.Address));

        if (query.LatitudeFrom.HasValue)
            filter = filter.AddFilter(location => location.Latitude >= query.LatitudeFrom.Value);

        if (query.LatitudeTo.HasValue)
            filter = filter.AddFilter(location => location.Latitude <= query.LatitudeTo.Value);

        if (query.LongitudeFrom.HasValue)
            filter = filter.AddFilter(location => location.Longitude >= query.LongitudeFrom.Value);

        if (query.LongitudeTo.HasValue)
            filter = filter.AddFilter(location => location.Longitude <= query.LongitudeTo.Value);

        return filter;
    }
}
