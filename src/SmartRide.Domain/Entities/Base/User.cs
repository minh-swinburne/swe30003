using Microsoft.EntityFrameworkCore;
using SmartRide.Domain.Entities.Join;
using SmartRide.Domain.Entities.Lookup;
using SmartRide.Domain.Enums;
using SmartRide.Domain.Events;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartRide.Domain.Entities.Base;

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

    public Identity? Identity { get; set; }

    public ICollection<Role> Roles { get; set; } = [];
    public ICollection<Ride> Rides { get; set; } = [];
    public ICollection<UserRole> UserRoles { get; set; } = [];
    public ICollection<Location> Locations { get; set; } = [];

    private ICollection<Vehicle>? _vehicles { get; set; } = [];

    private ICollection<License>? _licenses { get; set; } = [];

    [BackingField(nameof(_vehicles))]
    public ICollection<Vehicle>? Vehicles => IsDriver() ? _vehicles : null;

    [BackingField(nameof(_licenses))]
    public ICollection<License>? Licenses => IsDriver() ? _licenses : null;

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

    public override void OnSave(EntityState state)
    {
        base.OnSave(state);

        if (state == EntityState.Added)
        {
            // Assign Passenger role if no roles are assigned
            if (UserRoles.Count == 0)
            {
                UserRoles.Add(new UserRole
                {
                    UserId = Id,
                    RoleId = RoleEnum.Passenger
                });
            }

            AddDomainEvent(new UserCreatedEvent(this));
        }
        else if (state == EntityState.Modified)
        {
            AddDomainEvent(new UserUpdatedEvent(this));
        }
        else if (state == EntityState.Deleted)
        {
            AddDomainEvent(new UserDeletedEvent(this));
        }
    }
}
