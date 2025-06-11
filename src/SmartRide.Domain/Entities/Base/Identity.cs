using Microsoft.EntityFrameworkCore;
using SmartRide.Domain.Enums;
using SmartRide.Domain.Events;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartRide.Domain.Entities.Base;

[Index(nameof(UserId), IsUnique = true)]
[Index(nameof(NationalId), IsUnique = true)]
public class Identity : BaseEntity
{
    [Required]
    [Column(TypeName = "BINARY(16)")]
    public required Guid UserId { get; set; }

    [Required]
    [Column(TypeName = "TINYINT")]
    public IdentityStatusEnum Status { get; set; } = IdentityStatusEnum.Pending;  // Verification Status

    [Required]
    [Column(TypeName = "VARCHAR")]
    [StringLength(150)]
    public required string LegalName { get; set; }

    [Required]
    [Column(TypeName = "TINYINT")]
    public required IdentitySexEnum Sex { get; set; }

    [Required]
    [Column(TypeName = "DATE")]
    public required DateTime BirthDate { get; set; }

    [Required]
    [Column(TypeName = "VARCHAR")]
    [StringLength(25)]
    public required string NationalId { get; set; }

    [Required]
    [Column(TypeName = "VARCHAR")]
    [StringLength(60)]
    public required string Nationality { get; set; }

    [Required]
    [Column(TypeName = "VARCHAR")]
    [StringLength(255)]
    public required string Address { get; set; }

    [Required]
    [Column(TypeName = "VARCHAR")]
    [StringLength(100)]
    public required string City { get; set; }

    [Required]
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    public override void OnSave(EntityState state)
    {
        base.OnSave(state);

        if (state == EntityState.Added)
            AddDomainEvent(new IdentityCreatedEvent(this));
        else if (state == EntityState.Modified)
            AddDomainEvent(new IdentityUpdatedEvent(this));
        else if (state == EntityState.Deleted)
            AddDomainEvent(new IdentityDeletedEvent(this));
    }
}
