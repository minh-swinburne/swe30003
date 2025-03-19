using Microsoft.EntityFrameworkCore;
using SmartRide.Domain.Entities.Join;
using SmartRide.Domain.Entities.Lookup;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartRide.Domain.Entities;

[Index(nameof(Email), IsUnique = true)]
[Index(nameof(Phone), IsUnique = true)]
public class User : BaseEntity
{
    [Required]
    [Column(TypeName = "VARCHAR")]
    [StringLength(50)]
    public required string FirstName { get; set; }

    [Column(TypeName = "VARCHAR")]
    [StringLength(50)]
    public string? LastName { get; set; }

    [Required]
    [Column(TypeName = "VARCHAR")]
    [StringLength(255)]
    public required string Email { get; set; }

    [Required]
    [Column(TypeName = "VARCHAR")]
    [StringLength(45)]
    public required string Phone { get; set; }

    [Column(TypeName = "CHAR")]
    [StringLength(60)]
    public string? Password { get; set; }

    [Column(TypeName = "TEXT")]
    public string? Picture { get; set; }

    public ICollection<Role> Roles { get; set; } = [];
    public ICollection<UserRole> UserRoles { get; set; } = [];
}
