using MediatR;
using SmartRide.Application.Commands;
using SmartRide.Application.Commands.Users;
using SmartRide.Application.DTOs;
using SmartRide.Application.DTOs.Users;
using SmartRide.Application.Factories;
using SmartRide.Application.Interfaces;
using SmartRide.Application.Queries.Users;

namespace SmartRide.Application.Services;

public class UserService(IMediator mediator) : IUserService
{
    private readonly IMediator _mediator = mediator;

    public async Task<ListResponseDTO<ListUserResponseDTO>> ListUsersAsync(ListUserRequestDTO request)
    {
        var query = MediatRFactory.CreateQuery<ListUserQuery>(request);
        var result = await _mediator.Send(query);
        return new ListResponseDTO<ListUserResponseDTO>
        {
            Data = result,
            Count = result.Count
        };
    }

    public async Task<ResponseDTO<GetUserResponseDTO>> GetUserByIdAsync(GetUserByIdRequestDTO request)
    {
        var query = MediatRFactory.CreateQuery<GetUserByIdQuery>(request);
        var result = await _mediator.Send(query);
        return new ResponseDTO<GetUserResponseDTO> { Data = result };
    }

    public async Task<ResponseDTO<GetUserResponseDTO>> GetUserByEmailAsync(GetUserByEmailRequestDTO request)
    {
        var query = MediatRFactory.CreateQuery<GetUserByEmailQuery>(request);
        var result = await _mediator.Send(query);
        return new ResponseDTO<GetUserResponseDTO> { Data = result };
    }

    public async Task<ResponseDTO<GetUserResponseDTO>> GetUserByPhoneAsync(GetUserByPhoneRequestDTO request)
    {
        var query = MediatRFactory.CreateQuery<GetUserByPhoneQuery>(request);
        var result = await _mediator.Send(query);
        return new ResponseDTO<GetUserResponseDTO> { Data = result };
    }

    public async Task<ResponseDTO<CreateUserResponseDTO>> CreateUserAsync(CreateUserRequestDTO request)
    {
        var command = MediatRFactory.CreateCommand<CreateUserCommand>(request);
        var result = await _mediator.Send(command);
        await _mediator.Send(new SaveChangesCommand());
        return new ResponseDTO<CreateUserResponseDTO> { Data = result };
    }

    public async Task<ResponseDTO<UpdateUserResponseDTO>> UpdateUserAsync(UpdateUserRequestDTO request)
    {
        var command = MediatRFactory.CreateCommand<UpdateUserCommand>(request);
        var result = await _mediator.Send(command);
        await _mediator.Send(new SaveChangesCommand());
        return new ResponseDTO<UpdateUserResponseDTO> { Data = result };
    }

    public async Task<ResponseDTO<DeleteUserResponseDTO>> DeleteUserAsync(DeleteUserRequestDTO request)
    {
        var command = MediatRFactory.CreateCommand<DeleteUserCommand>(request);
        var result = await _mediator.Send(command);
        await _mediator.Send(new SaveChangesCommand());
        return new ResponseDTO<DeleteUserResponseDTO> { Data = result };
    }
}
