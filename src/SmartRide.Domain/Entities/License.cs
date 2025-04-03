using SmartRide.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartRide.Domain.Entities;

public class License : BaseEntity
{
    [Required]
    [Column(TypeName = "BINARY(16)")]
    public required Guid UserId { get; set; }

    [Required]
    [Column(TypeName = "TINYINT")]
    public required LicenseTypeEnum Type { get; set; }

    [Required]
    [Column(TypeName = "TINYINT")]
    public required LicenseStatusEnum Status { get; set; }

    [Required]
    [Column(TypeName = "DATE")]
    public required DateTime IssuedDate { get; set; }

    [Required]
    [ForeignKey(nameof(UserId))]
    public required User User { get; set; }
}
