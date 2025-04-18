namespace SmartRide.Application.DTOs.Users;

public class GetCurrentUserRequestDTO : BaseRequestDTO
{
    public required string AccessToken { get; init; }
}
