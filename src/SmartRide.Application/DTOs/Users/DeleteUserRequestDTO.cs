namespace SmartRide.Application.DTOs.Users;

public class DeleteUserRequestDTO : BaseRequestDTO
{
    public required Guid UserId { get; init; }
}
