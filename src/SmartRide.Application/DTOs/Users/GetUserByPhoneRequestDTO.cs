namespace SmartRide.Application.DTOs.Users;

public class GetUserByPhoneRequestDTO : BaseRequestDTO
{
    public required string Phone { get; init; }
}
