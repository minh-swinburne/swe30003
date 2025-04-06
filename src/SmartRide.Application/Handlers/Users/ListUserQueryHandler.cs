using AutoMapper;
using Humanizer;
using SmartRide.Application.DTOs.Users;
using SmartRide.Application.Interfaces;
using SmartRide.Application.Queries.Users;
using SmartRide.Common.Extensions;
using SmartRide.Common.Helpers;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;
using System.Linq.Expressions;
using System.Reflection;

namespace SmartRide.Application.Handlers.Users;

public class ListUserQueryHandler(IRepository<User> userRepository, IMapper mapper)
    : BaseQueryHandler<ListUserQuery, List<ListUserResponseDTO>>, IFilterableHandler<ListUserQuery, User>
{
    private readonly IRepository<User> _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;

    public override async Task<List<ListUserResponseDTO>> Handle(ListUserQuery query, CancellationToken cancellationToken)
    {
        Expression<Func<User, object>>? orderBy = null;
        Expression<Func<User, bool>>? filter = BuildFilter(query);

        if (!string.IsNullOrEmpty(query.OrderBy))
        {
            var propertyInfo = QueryHelper.GetProperty<User>(query.OrderBy);
            orderBy = QueryHelper.GetSortExpression<User>(propertyInfo!);
        }

        List<User> result = await _userRepository.GetWithFilterAsync<ListUserResponseDTO>(
            filter,
            orderBy: orderBy,
            ascending: query.Ascending,
            skip: query.PageSize * (query.PageNo - 1),
            limit: query.PageSize,
            cancellationToken: cancellationToken
        );

        return _mapper.Map<List<ListUserResponseDTO>>(result);
    }

    public Expression<Func<User, bool>>? BuildFilter(ListUserQuery query)
    {
        Expression<Func<User, bool>>? filter = null;

        if (!string.IsNullOrWhiteSpace(query.FirstName))
            filter = filter.AddFilter(user => user.FirstName.Contains(query.FirstName, StringComparison.CurrentCultureIgnoreCase));

        if (!string.IsNullOrWhiteSpace(query.LastName))
            filter = filter.AddFilter(user => user.LastName != null && user.LastName.Contains(query.LastName, StringComparison.CurrentCultureIgnoreCase));

        if (!string.IsNullOrWhiteSpace(query.Email))
            filter = filter.AddFilter(user => user.Email != null && user.Email.Contains(query.Email, StringComparison.CurrentCultureIgnoreCase));

        if (!string.IsNullOrWhiteSpace(query.Phone))
            filter = filter.AddFilter(user => user.Phone != null && user.Phone.Contains(query.Phone, StringComparison.CurrentCultureIgnoreCase));

        if (query.Roles is { Count: > 0 })
        {
            if (query.MatchAllRoles)
                // Ensure the user has *all* the queryed roles
                filter = filter.AddFilter(user => query.Roles.All(role => user.UserRoles.Any(ur => ur.RoleId == role)));
            else
                // Ensure the user has *at least one* of the queryed roles
                filter = filter.AddFilter(user => user.UserRoles.Any(ur => query.Roles.Contains(ur.RoleId)));
        }

        return filter;
    }
}
