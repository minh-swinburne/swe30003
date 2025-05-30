﻿using AutoMapper;
using MediatR;
using SmartRide.Application.Commands;
using SmartRide.Application.Commands.Users;
using SmartRide.Application.DTOs;
using SmartRide.Application.DTOs.Users;
using SmartRide.Application.Factories;
using SmartRide.Application.Interfaces;
using SmartRide.Application.Queries.Users;
using SmartRide.Common.Responses;
using SmartRide.Common.Responses.Errors;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Application.Services;

public class UserService(IMediator mediator, IMapper mapper, IJwtService jwtService) : IUserService
{
    private readonly IMediator _mediator = mediator;
    private readonly IMapper _mapper = mapper;
    private readonly IJwtService _jwtService = jwtService;

    public async Task<ListResponseDTO<ListUserResponseDTO>> ListUsersAsync(ListUserRequestDTO request)
    {
        try
        {
            var query = MediatRFactory.CreateQuery<ListUserQuery>(request);
            var result = await _mediator.Send(query);
            return new ListResponseDTO<ListUserResponseDTO>
            {
                Data = result,
                Count = result.Count
            };
        }
        catch (Exception ex)
        {
            return new ListResponseDTO<ListUserResponseDTO>
            {
                Info = new ResponseInfo { Code = "LIST_USERS_ERROR", Message = ex.Message }
            };
        }
    }

    public async Task<ResponseDTO<GetUserResponseDTO>> GetCurrentUserAsync(GetCurrentUserRequestDTO request)
    {
        try
        {
            var payload = _jwtService.DecodeToken(request.AccessToken);

            if (payload == null || payload.Count == 0)
                return new ResponseDTO<GetUserResponseDTO>
                {
                    Info = AuthErrors.TOKEN_EMPTY
                };

            var userId = payload
                .Where(c => c.Key == "sub")
                .Select(c => c.Value)
                .FirstOrDefault()?
                .ToString();

            if (string.IsNullOrEmpty(userId))
                return new ResponseDTO<GetUserResponseDTO>
                {
                    Info = UserErrors.ID_EMPTY
                };

            var query = new GetUserByIdQuery { UserId = Guid.Parse(userId) };
            var result = await _mediator.Send(query);
            return new ResponseDTO<GetUserResponseDTO> { Data = result };
        }
        catch (Exception ex)
        {
            return new ResponseDTO<GetUserResponseDTO>
            {
                Info = new ResponseInfo { Code = "GET_CURRENT_USER_ERROR", Message = ex.Message }
            };
        }
    }

    public async Task<ResponseDTO<GetUserResponseDTO>> GetUserByIdAsync(GetUserByIdRequestDTO request)
    {
        try
        {
            var query = MediatRFactory.CreateQuery<GetUserByIdQuery>(request);
            var result = await _mediator.Send(query);
            return new ResponseDTO<GetUserResponseDTO> { Data = result };
        }
        catch (Exception ex)
        {
            return new ResponseDTO<GetUserResponseDTO>
            {
                Info = new ResponseInfo { Code = "GET_USER_BY_ID_ERROR", Message = ex.Message }
            };
        }
    }

    public async Task<ResponseDTO<GetUserResponseDTO>> GetUserByEmailAsync(GetUserByEmailRequestDTO request)
    {
        try
        {
            var query = MediatRFactory.CreateQuery<GetUserByEmailQuery>(request);
            var result = await _mediator.Send(query);
            return new ResponseDTO<GetUserResponseDTO> { Data = result };
        }
        catch (Exception ex)
        {
            return new ResponseDTO<GetUserResponseDTO>
            {
                Info = new ResponseInfo { Code = "GET_USER_BY_EMAIL_ERROR", Message = ex.Message }
            };
        }
    }

    public async Task<ResponseDTO<GetUserResponseDTO>> GetUserByPhoneAsync(GetUserByPhoneRequestDTO request)
    {
        try
        {
            var query = MediatRFactory.CreateQuery<GetUserByPhoneQuery>(request);
            var result = await _mediator.Send(query);
            return new ResponseDTO<GetUserResponseDTO> { Data = result };
        }
        catch (Exception ex)
        {
            return new ResponseDTO<GetUserResponseDTO>
            {
                Info = new ResponseInfo { Code = "GET_USER_BY_PHONE_ERROR", Message = ex.Message }
            };
        }
    }

    public async Task<ResponseDTO<CreateUserResponseDTO>> CreateUserAsync(CreateUserRequestDTO request)
    {
        try
        {
            var command = MediatRFactory.CreateCommand<CreateUserCommand>(request);
            var result = await _mediator.Send(command);
            await _mediator.Send(new SaveChangesCommand());
            return new ResponseDTO<CreateUserResponseDTO> { Data = result };
        }
        catch (Exception ex)
        {
            return new ResponseDTO<CreateUserResponseDTO>
            {
                Info = new ResponseInfo { Code = "CREATE_USER_ERROR", Message = ex.Message }
            };
        }
    }

    public async Task<ResponseDTO<UpdateUserResponseDTO>> UpdateUserAsync(UpdateUserRequestDTO request)
    {
        try
        {
            var command = MediatRFactory.CreateCommand<UpdateUserCommand>(request);
            var result = await _mediator.Send(command);
            await _mediator.Send(new SaveChangesCommand());
            return new ResponseDTO<UpdateUserResponseDTO> { Data = result };
        }
        catch (Exception ex)
        {
            return new ResponseDTO<UpdateUserResponseDTO>
            {
                Info = new ResponseInfo { Code = "UPDATE_USER_ERROR", Message = ex.Message }
            };
        }
    }

    public async Task<ResponseDTO<DeleteUserResponseDTO>> DeleteUserAsync(DeleteUserRequestDTO request)
    {
        try
        {
            var command = MediatRFactory.CreateCommand<DeleteUserCommand>(request);
            var result = await _mediator.Send(command);
            await _mediator.Send(new SaveChangesCommand());
            return new ResponseDTO<DeleteUserResponseDTO> { Data = result };
        }
        catch (Exception ex)
        {
            return new ResponseDTO<DeleteUserResponseDTO>
            {
                Info = new ResponseInfo { Code = "DELETE_USER_ERROR", Message = ex.Message }
            };
        }
    }
}
