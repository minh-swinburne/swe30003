﻿using AutoMapper;
using Humanizer;
using SmartRide.Application.DTOs.Users;
using SmartRide.Application.Interfaces;
using SmartRide.Common.Extensions;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;
using System.Linq.Expressions;
using System.Reflection;

namespace SmartRide.Application.Queries.Users.Handlers;

public class ListUserQueryHandler(IRepository<User> userRepository, IMapper mapper)
    : BaseQueryHandler<ListUserQuery, List<ListUserResponseDTO>>, IFilterableHandler<ListUserQuery, User>
{
    private readonly IRepository<User> _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;

    public override async Task<List<ListUserResponseDTO>> Handle(ListUserQuery query, CancellationToken cancellationToken)
    {
        Expression<Func<User, object>>? orderBy = null;
        Expression<Func<User, bool>>? filter = BuildFilter(query);
        //Expression<Func<User, ListUserResponseDTO>>? select = u => new ListUserResponseDTO
        //{
        //    Id = u.Id,
        //    FirstName = u.FirstName,
        //    LastName = u.LastName,
        //    Email = u.Email,
        //    Phone = u.Phone,
        //    Picture = u.Picture,
        //    Roles = u.Roles.Select(r => r.Id).ToList(),
        //};

        if (!string.IsNullOrEmpty(query.OrderBy))
        {
            var propertyInfo = typeof(User).GetProperty(query.OrderBy.Pascalize(), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                ?? throw new InvalidFilterCriteriaException($"Property {query.OrderBy} does not exist in {nameof(User)}.");

            var parameter = Expression.Parameter(typeof(User), "u");
            var propertyAccess = Expression.Property(parameter, propertyInfo);
            orderBy = (Expression<Func<User, object>>)Expression.Lambda(propertyAccess, parameter);
        }

        List<User> result = await _userRepository.GetWithFilterAsync<ListUserResponseDTO>(
            filter,
            //select,
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
