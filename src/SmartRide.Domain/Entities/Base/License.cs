using Microsoft.EntityFrameworkCore;
using SmartRide.Domain.Enums;
using SmartRide.Domain.Events;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartRide.Domain.Entities.Base;

[Index(nameof(Number), IsUnique = true)]
public class License : BaseEntity
{
    [Required]
    [Column(TypeName = "BINARY(16)")]
    public required Guid UserId { get; set; }

    [Required]
    [Column(TypeName = "VARCHAR(50)")]
    [StringLength(50)]
    public string Number { get; set; } = null!;

    [Required]
    [Column(TypeName = "TINYINT")]
    public required LicenseTypeEnum Type { get; set; }

    [Required]
    [Column(TypeName = "TINYINT")]
    public LicenseStatusEnum Status { get; set; } = LicenseStatusEnum.Active;  // License Validity Status

    [Required]
    [Column(TypeName = "DATE")]
    public required DateTime IssuedDate { get; set; }

    [Required]
    [Column(TypeName = "VARCHAR(100)")]
    [StringLength(100)]
    public string IssuedCountry { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    public override void OnSave(EntityState state)
    {
        base.OnSave(state);

        if (state == EntityState.Added)
        {
            AddDomainEvent(new LicenseCreatedEvent(this));
        }
        else if (state == EntityState.Modified)
        {
            AddDomainEvent(new LicenseUpdatedEvent(this));
        }
        else if (state == EntityState.Deleted)
        {
            AddDomainEvent(new LicenseDeletedEvent(this));
        }
    }
}
