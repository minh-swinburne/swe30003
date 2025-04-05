using SmartRide.Domain.Enums;

namespace SmartRide.Application.DTOs.Users;

public class CreateUserRequestDTO : BaseDTO
{
    public required string FirstName { get; set; }
    public string? LastName { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public required string Password { get; set; }
    public string? Picture { get; set; }
}
