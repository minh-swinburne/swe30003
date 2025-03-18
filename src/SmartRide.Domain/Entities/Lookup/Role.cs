using SmartRide.Domain.Entities.Join;
using SmartRide.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartRide.Domain.Entities.Lookup;

public class Role : LookupEntity
{
    [Key]
    [Column(TypeName = "int")]
    public new RoleEnum Id { get; set; }

    public ICollection<User> Users { get; set; } = [];
    public ICollection<UserRole> UserRoles { get; set; } = [];
}
