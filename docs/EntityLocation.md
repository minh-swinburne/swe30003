# Question

Should I create an entity for Location, specifying its address in text, its coordinate (X, Y) in map, and possibly link it to a User? This will be used by a Ride entity (to track the pickup location, stops and destination)... or should I just use varchar fields to store address in text?

---

# Answer

It is better to create a `Location` entity. This approach provides flexibility, reusability, and normalization. Here's why:

1. **Reusability**: A `Location` entity can be reused across multiple entities like `User`, `Ride`, or even `Vehicle` if needed.
2. **Normalization**: Storing location details in a separate table avoids redundancy and ensures consistency.
3. **Extensibility**: You can easily add more fields (e.g., postal code, country) or relationships (e.g., linking to a map service) in the future.
4. **Querying**: It simplifies querying for rides or users based on location.

### Suggested Changes

1. Create a `Location` entity with fields for address, coordinates, and optional links to a `User`.
2. Update the `Ride` entity to reference `Location` for pickup, stops, and destination.

### [Location.cs](file:///c%3A/Users/xmas4/OneDrive%20-%20Swinburne%20University/BA-CS/SWE30003%20-%20Software%20Architectures%20and%20Design/swe30003/src/SmartRide.Domain/Entities/Location.cs)

Define the `Location` entity.

```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartRide.Domain.Entities.Base;

public class Location : BaseEntity
{
    [Required]
    [Column(TypeName = "VARCHAR")]
    [StringLength(255)]
    public required string Address { get; set; }

    [Column(TypeName = "DOUBLE")]
    public double? Latitude { get; set; }

    [Column(TypeName = "DOUBLE")]
    public double? Longitude { get; set; }

    public Guid? UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }
}
```

### [Ride.cs](file:///c%3A/Users/xmas4/OneDrive%20-%20Swinburne%20University/BA-CS/SWE30003%20-%20Software%20Architectures%20and%20Design/swe30003/src/SmartRide.Domain/Entities/Ride.cs)

Update the `Ride` entity to reference `Location`.

```csharp


Made changes.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartRide.Domain.Entities.Base;

public class Ride : BaseEntity
{
    // ...existing code...

    [Required]
    public Guid PickupLocationId { get; set; }

    [ForeignKey(nameof(PickupLocationId))]
    public required Location PickupLocation { get; set; }

    public Guid? DestinationLocationId { get; set; }

    [ForeignKey(nameof(DestinationLocationId))]
    public Location? DestinationLocation { get; set; }

    public List<Location>? Stops { get; set; }
}
```

### [SmartRideDbContext.cs](file:///c%3A/Users/xmas4/OneDrive%20-%20Swinburne%20University/BA-CS/SWE30003%20-%20Software%20Architectures%20and%20Design/swe30003/src/SmartRide.Infrastructure/Persistence/SmartRideDbContext.cs)

Add `Location` and update relationships.

```csharp


Made changes.

// ...existing code...

modelBuilder.Entity<Location>()
    .HasOne(l => l.User)
    .WithMany(u => u.Locations)
    .HasForeignKey(l => l.UserId)
    .OnDelete(DeleteBehavior.SetNull);

modelBuilder.Entity<Ride>()
    .HasOne(r => r.PickupLocation)
    .WithMany()
    .HasForeignKey(r => r.PickupLocationId)
    .OnDelete(DeleteBehavior.Restrict);

modelBuilder.Entity<Ride>()
    .HasOne(r => r.DestinationLocation)
    .WithMany()
    .HasForeignKey(r => r.DestinationLocationId)
    .OnDelete(DeleteBehavior.Restrict);
```

This design ensures flexibility and scalability for future requirements.

Made changes.
