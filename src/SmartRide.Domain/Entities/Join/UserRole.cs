using Microsoft.EntityFrameworkCore;
using SmartRide.Domain.Entities.Lookup;
using SmartRide.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartRide.Domain.Entities.Join;

[PrimaryKey(nameof(UserId), nameof(RoleId))]
public class UserRole : JoinEntity
{
    [Column(TypeName = "BINARY(16)", Order = 0)]
    public required Guid UserId { get; set; }

    [Column(TypeName = "TINYINT", Order = 1)]
    public required RoleEnum RoleId { get; set; }

    [Required]
    [ForeignKey(nameof(UserId))]
    public required User User { get; set; }

    [Required]
    [ForeignKey(nameof(RoleId))]
    public required Role Role { get; set; }
}
