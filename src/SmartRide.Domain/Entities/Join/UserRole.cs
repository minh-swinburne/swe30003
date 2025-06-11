using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Entities.Lookup;
using SmartRide.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartRide.Domain.Entities.Join;

public class UserRole : JoinEntity
{
    [Column(TypeName = "BINARY(16)", Order = 0)]
    public required Guid UserId { get; set; }

    [Column(TypeName = "TINYINT", Order = 1)]
    public required RoleEnum RoleId { get; set; }

    [Required]
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(RoleId))]
    public Role Role { get; set; } = null!;
}
