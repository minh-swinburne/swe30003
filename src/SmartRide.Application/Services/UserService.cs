﻿using MediatR;
using SmartRide.Application.Commands;
using SmartRide.Application.DTOs;
using SmartRide.Application.DTOs.Users;
using SmartRide.Application.Factories;
using SmartRide.Application.Interfaces;
using SmartRide.Application.Queries.Users;

namespace SmartRide.Application.Services;

public class UserService(IMediator mediator) : IUserService
{
    private readonly IMediator _mediator = mediator;

    public async Task<ListResponseDTO<ListUserResponseDTO>> GetAllUsersAsync(ListUserRequestDTO request)
    {
        var query = MediatRFactory.CreateQuery<ListUserQuery>(request);
        return await _mediator.Send(query);
    }

    public async Task<ResponseDTO<GetUserByIdResponseDTO>> GetUserByIdAsync(GetUserByIdRequestDTO request)
    {
        var query = MediatRFactory.CreateQuery<GetUserByIdQuery>(request);
        var result = await _mediator.Send(query);
        return new ResponseDTO<GetUserByIdResponseDTO> { Data = result };
    }

    public async Task<ResponseDTO<CreateUserResponseDTO>> CreateUserAsync(CreateUserRequestDTO request)
    {
        var command = MediatRFactory.CreateCommand<CreateUserCommand>(request);
        return await _mediator.Send(command);
    }
}
