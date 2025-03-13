using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Entities.Join;

namespace SmartRide.Domain.Entities;

public class User : Entity
{
    public required string FirstName { get; set; }
    public string? LastName { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public string? Password { get; set; }
    public string? Picture { get; set; }

    public ICollection<Role> Roles { get; set; } = [];
    public ICollection<UserRole> UserRoles { get; set; } = [];
}
