using Microsoft.EntityFrameworkCore;
using SmartRide.Domain.Entities.Lookup;
using SmartRide.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartRide.Domain.Entities.Join;

[PrimaryKey(nameof(UserId), nameof(RoleId))]
public class UserRole : JoinEntity
{
    [Column(TypeName = "char", Order = 0)]
    [StringLength(36)]
    public required string UserId { get; set; }

    [Column(TypeName = "int", Order = 1)]
    public required RoleEnum RoleId { get; set; }

    [ForeignKey(nameof(UserId))]
    public required User User { get; set; }

    [ForeignKey(nameof(RoleId))]
    public required Role Role { get; set; }
}
