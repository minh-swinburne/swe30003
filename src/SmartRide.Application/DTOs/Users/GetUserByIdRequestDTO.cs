namespace SmartRide.Application.DTOs.Users;

public class GetUserByIdRequestDTO : BaseDTO
{
    public required Guid UserId { get; init; }
}