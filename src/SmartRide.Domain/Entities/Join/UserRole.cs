using Microsoft.EntityFrameworkCore;
using SmartRide.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartRide.Domain.Entities.Join;

[PrimaryKey(nameof(UserId), nameof(RoleId))]
public class UserRole
{
    [Column(TypeName = "char", Order = 0)]
    [StringLength(36)]
    public required string UserId { get; set; }

    [Column(TypeName = "int", Order = 1)]
    public required UserRoleEnum RoleId { get; set; }

    public required User User { get; set; }
    public required Role Role { get; set; }
}
