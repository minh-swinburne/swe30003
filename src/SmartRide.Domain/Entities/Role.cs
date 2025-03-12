using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Enums;

namespace SmartRide.Domain.Entities;

public class Role : LookupEntity
{
    public new UserRoleEnum Id { get; set; }

    public ICollection<User> Users { get; set; } = [];
}
