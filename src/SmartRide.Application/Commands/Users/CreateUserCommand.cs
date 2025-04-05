using SmartRide.Application.DTOs.Users;

namespace SmartRide.Application.Commands.Users;

public class CreateUserCommand : BaseCommand<CreateUserResponseDTO>
{
    public string FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? Picture { get; set; }
}
