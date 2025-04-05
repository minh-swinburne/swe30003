namespace SmartRide.Application.DTOs.Users;

public class CreateUserRequestDTO : BaseRequestDTO
{
    public required string FirstName { get; set; }
    public string? LastName { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public string? Password { get; set; }
    public string? Picture { get; set; }
}
