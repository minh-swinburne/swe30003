using SmartRide.Common.Constants;
using SmartRide.Domain.Entities.Join;
using SmartRide.Domain.Entities.Lookup;
using SmartRide.Domain.Enums;
using SmartRide.Domain.Events;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartRide.Domain.Entities.Base;

public class User : BaseEntity
{
    [Required]
    [Column(TypeName = "VARCHAR")]
    [StringLength(UserConstants.NameMaxLength)]
    public required string FirstName { get; set; }

    [Column(TypeName = "VARCHAR")]
    [StringLength(UserConstants.NameMaxLength)]
    public string? LastName { get; set; }

    [Required]
    [Column(TypeName = "VARCHAR")]
    [StringLength(UserConstants.EmailMaxLength)]
    [RegularExpression(UserConstants.EmailPattern)]
    public required string Email { get; set; }

    [Required]
    [Column(TypeName = "VARCHAR")]
    [StringLength(UserConstants.PhoneMaxLength)]
    [RegularExpression(UserConstants.PhonePattern)]
    public required string Phone { get; set; }

    [Column(TypeName = "VARCHAR")]
    [StringLength(UserConstants.PasswordMaxLength, MinimumLength = UserConstants.PasswordMinLength)]
    [RegularExpression(UserConstants.PasswordPattern)]
    public string? Password { get; set; }

    [Column(TypeName = "TEXT")]
    public string? Picture { get; set; }

    public Identity? Identity { get; set; }

    public ICollection<Role> Roles { get; set; } = [];
    public ICollection<Ride> Rides { get; set; } = [];
    public ICollection<UserRole> UserRoles { get; set; } = [];
    public ICollection<Location> Locations { get; set; } = [];

    private ICollection<Vehicle> _vehicles = [];
    private ICollection<License> _licenses = [];

    public ICollection<Vehicle> Vehicles
    {
        get => IsDriver() ? _vehicles : [];
        private set => _vehicles = value;
    }

    public ICollection<License> Licenses
    {
        get => IsDriver() ? _licenses : [];
        private set => _licenses = value;
    }

    public bool IsDriver() => UserRoles.Any(ur => ur.RoleId == RoleEnum.Driver);

    public bool IsPassenger() => UserRoles.Any(ur => ur.RoleId == RoleEnum.Passenger);

    public bool IsManager() => UserRoles.Any(ur => ur.RoleId == RoleEnum.Manager);

    public IEnumerable<Ride> ActiveRides() => Rides.Where(r => r.Status == RideStatusEnum.Picking || r.Status == RideStatusEnum.Travelling);

    public override void OnSave(string state)
    {
        base.OnSave(state);

        if (state == "Added")
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
        else if (state == "Modified")
            AddDomainEvent(new UserUpdatedEvent(this));
        else if (state == "Deleted")
            AddDomainEvent(new UserDeletedEvent(this));
    }
}
