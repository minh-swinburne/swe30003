using Microsoft.EntityFrameworkCore;
using SmartRide.Domain.Entities.Join;
using SmartRide.Domain.Entities.Lookup;
using SmartRide.Domain.Enums;
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

    [Column(TypeName = "VARCHAR")]
    [StringLength(150)]
    public string? Password { get; set; }

    [Column(TypeName = "TEXT")]
    public string? Picture { get; set; }

    [Column(TypeName = "BINARY(16)")]
    public Guid? IdentityId { get; set; }

    public Identity? Identity { get; set; }

    public List<Role> Roles { get; set; } = [];
    public List<UserRole> UserRoles { get; set; } = [];

    private List<Vehicle>? _vehicles { get; set; }

    [BackingField(nameof(_vehicles))]
    public List<Vehicle>? Vehicles => IsDriver() ? _vehicles : null;

    private List<License>? _licenses { get; set; }

    [BackingField(nameof(_licenses))]
    public List<License>? Licenses => IsDriver() ? _licenses : null;

    public List<Location>? Locations { get; set; } = [];

    public bool IsDriver()
    {
        return UserRoles.Any(ur => ur.RoleId == RoleEnum.Driver);
    }

    public bool IsPassenger()
    {
        return UserRoles.Any(ur => ur.RoleId == RoleEnum.Passenger);
    }

    public bool IsManager()
    {
        return UserRoles.Any(ur => ur.RoleId == RoleEnum.Manager);
    }
}
