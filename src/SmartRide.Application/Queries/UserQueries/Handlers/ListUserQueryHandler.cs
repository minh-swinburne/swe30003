using MediatR;
using Humanizer;
using AutoMapper;
using System.Linq.Expressions;
using System.Reflection;
using SmartRide.Application.DTOs;
using SmartRide.Application.DTOs.User;
using SmartRide.Common.Extensions;
using SmartRide.Domain.Entities;
using SmartRide.Domain.Interfaces;
using SmartRide.Application.Interfaces;

namespace SmartRide.Application.Queries.UserQueries.Handlers;

public class ListUserQueryHandler(IRepository<User> userRepository, IMapper mapper) 
    : IRequestHandler<ListUserQuery, ListResponseDTO<ListUserResponseDTO>>, IFilterableHandler<ListUserQuery, User>
{
    private readonly IRepository<User> _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ListResponseDTO<ListUserResponseDTO>> Handle(ListUserQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<User, object>>? orderBy = null;
        Expression<Func<User, bool>>? filter = BuildFilter(request);
        Expression<Func<User, ListUserResponseDTO>>? select = u => new ListUserResponseDTO
        {
            Id = u.Id,
            FirstName = u.FirstName,
            LastName = u.LastName,
            Email = u.Email,
            Phone = u.Phone,
            Picture = u.Picture,
        };

        if (!string.IsNullOrEmpty(request.OrderBy))
        {
            var propertyInfo = typeof(User).GetProperty(request.OrderBy.Pascalize(), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                ?? throw new InvalidFilterCriteriaException($"Property {request.OrderBy} does not exist in {nameof(User)}.");

            var parameter = Expression.Parameter(typeof(User), "u");
            var propertyAccess = Expression.Property(parameter, propertyInfo);
            orderBy = (Expression<Func<User, object>>) Expression.Lambda(propertyAccess, parameter);
        }

        var result = await _userRepository.GetWithFilterAsync<ListUserResponseDTO>(
            filter,
            //select,
            orderBy: orderBy,
            ascending: request.Ascending,
            skip: request.PageSize * (request.PageNo - 1),
            limit: request.PageSize,
            cancellationToken: cancellationToken
        );

        return new ListResponseDTO<ListUserResponseDTO>
        {
            Data = [],
            Count = 0
        };
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

        return filter;
    }
}
