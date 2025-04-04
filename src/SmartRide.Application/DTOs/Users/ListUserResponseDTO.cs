using SmartRide.Domain.Enums;

namespace SmartRide.Application.DTOs.Users;

public class ListUserResponseDTO
{
    public required Guid UserId { get; set; }
    public required string FirstName { get; set; }
    public string? LastName { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public string? Picture { get; set; }
    public required List<RoleEnum> Roles { get; set; }
    //public string? Address { get; set; }  // From identity - implement later
}
