using SmartRide.Domain.Enums;

namespace SmartRide.Domain.Entities.Join;

public class UserRole
{
    public required string UserId { get; set; }
    public required UserRoleEnum RoleId { get; set; }

    public required User User { get; set; }
    public required Role Role { get; set; }
}
