namespace SmartRide.Application.DTOs.Users;

public abstract class BaseUserResponseDTO
{
    public required Guid UserId { get; set; }
}
