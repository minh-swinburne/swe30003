using SmartRide.Application.DTOs.Users;

namespace SmartRide.Application.Commands.Users;

public class DeleteUserCommand : BaseCommand<DeleteUserResponseDTO>
{
    public Guid UserId { get; set; }
}
