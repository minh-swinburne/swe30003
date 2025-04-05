namespace SmartRide.Application.DTOs.Users;

public class GetUserByIdRequestDTO : BaseRequestDTO
{
    public required Guid UserId { get; init; }
}