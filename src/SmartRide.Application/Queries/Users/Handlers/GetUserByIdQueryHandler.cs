﻿using AutoMapper;
using SmartRide.Application.DTOs.Users;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Application.Queries.Users.Handlers;

public class GetUserByIdQueryHandler(IRepository<User> userRepository, IMapper mapper)
    : BaseQueryHandler<GetUserByIdQuery, GetUserResponseDTO>
{
    private readonly IRepository<User> _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;

    public override async Task<GetUserResponseDTO> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(query.UserId, cancellationToken);
        return _mapper.Map<GetUserResponseDTO>(user);
    }
}
