using SmartRide.Application.DTOs.Users;

namespace SmartRide.Application.Commands.Users;

public class UpdateUserCommand : BaseCommand<UpdateUserResponseDTO>
{
    public Guid UserId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Password { get; set; }
    public string? Picture { get; set; }
}
