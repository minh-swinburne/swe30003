using SmartRide.Application.Commands.Users;
using SmartRide.Application.DTOs.Users;
using SmartRide.Common.Exceptions;
using SmartRide.Common.Responses.Errors;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Application.Handlers.Users;

public class DeleteUserCommandHandler(IRepository<User> userRepository)
    : BaseCommandHandler<DeleteUserCommand, DeleteUserResponseDTO>
{
    private readonly IRepository<User> _userRepository = userRepository;

    public override async Task<DeleteUserResponseDTO> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(command.UserId, cancellationToken: cancellationToken)
            ?? throw new BaseException(UserErrors.Module, UserErrors.ID_NOT_FOUND.FormatMessage(("UserId", command.UserId)));

        await _userRepository.DeleteAsync(user.Id, cancellationToken);

        return new DeleteUserResponseDTO
        {
            UserId = user.Id,
            Success = true,
        };
    }
}
