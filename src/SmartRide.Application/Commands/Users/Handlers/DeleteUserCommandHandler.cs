using SmartRide.Application.DTOs.Users;
using SmartRide.Common.Exceptions;
using SmartRide.Common.Responses.Errors;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Application.Commands.Users.Handlers;

public class DeleteUserCommandHandler(IRepository<User> userRepository)
    : BaseCommandHandler<DeleteUserCommand, DeleteUserResponseDTO>
{
    private readonly IRepository<User> _userRepository = userRepository;

    public override async Task<DeleteUserResponseDTO> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(command.UserId, cancellationToken)
            ?? throw new BaseException(UserErrors.Module, UserErrors.USER_NOT_FOUND.FormatMessage(command.UserId));

        await _userRepository.DeleteAsync(user.Id, cancellationToken);

        return new DeleteUserResponseDTO
        {
            UserId = user.Id,
            Success = true,
        };
    }
}
