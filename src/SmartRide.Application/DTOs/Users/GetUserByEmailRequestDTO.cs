namespace SmartRide.Application.DTOs.Users;

public class GetUserByEmailRequestDTO : BaseRequestDTO
{
    public required string Email { get; init; }
}
