# Question

in the Vehicle entity, both the UserId & VehicleTypeId (FK columns) and User & VehicleType (FK references) are required, leading to creating a Vehicle requiring passing both the references and their IDs. Is this a recommended implementation? If not then what is the better approach? Because VehicleType is a lookup entity, which means there shouldn't be creation of new instances of that entities, only "get" given ID. How to get them btw?
```cs
using Microsoft.EntityFrameworkCore;
using SmartRide.Domain.Entities.Lookup;
using SmartRide.Domain.Enums;
using SmartRide.Domain.Events;
using SmartRide.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartRide.Domain.Entities;

[Index(nameof(Vin), IsUnique = true)]
[Index(nameof(Plate), IsUnique = true)]
public class Vehicle : BaseEntity
{
    [Required]
    [Column(TypeName = "BINARY(16)", Order = 0)]
    public required Guid UserId { get; set; }

    [Required]
    [ForeignKey(nameof(UserId))]    // can be removed if the property name is UserId
    public required User User { get; set; }

    [Required]
    [Column(TypeName = "TINYINT", Order = 1)]
    public required VehicleTypeEnum VehicleTypeId { get; set; }

    [Required]
    [ForeignKey(nameof(VehicleTypeId))]
    public required VehicleType VehicleType { get; set; }

    [Required]
    [Column(TypeName = "CHAR")]
    [StringLength(17)]
    public required string Vin { get; set; }

    [Required]
    [Column(TypeName = "VARCHAR")]
    [StringLength(10)]
    public required string Plate { get; set; }

    [Required]
    [Column(TypeName = "VARCHAR")]
    [StringLength(50)]
    public required string Make { get; set; }

    [Required]
    [Column(TypeName = "VARCHAR")]
    [StringLength(50)]
    public required string Model { get; set; }

    [Required]
    [Column(TypeName = "INT")]
    public required int Year { get; set; }

    [Required]
    [Column(TypeName = "DATE")]
    public required DateTime RegisteredDate { get; set; }

    public override void OnSave(EntityState state)
    {
        base.OnSave(state);
        // Add domain events based on the entity state
        if (state == EntityState.Added)
        {
            AddDomainEvent(new VehicleCreatedEvent(this));
        }
        else if (state == EntityState.Modified)
        {
            AddDomainEvent(new VehicleUpdatedEvent(this));
        }
        else if (state == EntityState.Deleted)
        {
            AddDomainEvent(new VehicleDeletedEvent(this));
        }
    }
}
```

When trying to create Vehicle:
```cs
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            Email = "johndoe@example.com",
            Phone = "1234567890",
        };
        var vehicle = new Vehicle
        {
            Id = Guid.NewGuid(),
            User = user,
            UserId = user.Id,
            VehicleTypeId = VehicleTypeEnum.SmallCar,
            // throwing error because VehicleType is not provided
            Vin = "1HGCM82633A123456",
            Plate = "ABC123",
            Make = "Toyota",
            Model = "Corolla",
            Year = 2020,
            RegisteredDate = DateTime.UtcNow
        };
```

---

# Answer

Your current approach requires both the **ID** and the **navigation property (entity reference)** for `User` and `VehicleType`, which can be cumbersome. Here's how you can improve it:

### 🔹 **Best Approach**
For **lookup entities** (like `VehicleType`), you don’t need to store a full reference (`VehicleType VehicleType`) in the `Vehicle` entity. Instead, just store the `VehicleTypeId`. Then, when retrieving a vehicle, you can **include** the related `VehicleType` if needed.

### ✅ **Updated Vehicle Entity**
```csharp
public class Vehicle : BaseEntity
{
    [Required]
    [Column(TypeName = "BINARY(16)")]
    public required Guid UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }  // Nullable to avoid requiring full User object at creation

    [Required]
    [Column(TypeName = "TINYINT")]
    public required VehicleTypeEnum VehicleTypeId { get; set; } // Store ID only

    public VehicleType? VehicleType { get; set; } // Optional, can be included when needed

    [Required]
    [Column(TypeName = "CHAR", Length = 17)]
    public required string Vin { get; set; }

    [Required]
    [Column(TypeName = "VARCHAR", Length = 10)]
    public required string Plate { get; set; }

    [Required]
    [Column(TypeName = "VARCHAR", Length = 50)]
    public required string Make { get; set; }

    [Required]
    [Column(TypeName = "VARCHAR", Length = 50)]
    public required string Model { get; set; }

    [Required]
    [Column(TypeName = "INT")]
    public required int Year { get; set; }

    [Required]
    [Column(TypeName = "DATE")]
    public required DateTime RegisteredDate { get; set; }
}
```

---

### 🔹 **How to Get the Lookup Entity (`VehicleType`)**
Since `VehicleType` is a lookup entity, you don’t create new instances. Instead, you **fetch** it based on the `VehicleTypeId`:

```csharp
var vehicleType = await _context.VehicleTypes.FindAsync((byte)VehicleTypeEnum.SmallCar);
```
💡 **Note:**
- `VehicleTypeEnum.SmallCar` is an `enum`, so you **cast it to `byte`** (since `VehicleTypeId` is stored as `TINYINT`).
- If `FindAsync` returns `null`, it means the provided ID is invalid.

---

### 🔹 **Updated Vehicle Creation Code**
With the new design, you don’t need to set `VehicleType`, only `VehicleTypeId`:

```csharp
var user = new User
{
    Id = Guid.NewGuid(),
    FirstName = "John",
    LastName = "Doe",
    Email = "johndoe@example.com",
    Phone = "1234567890",
};

var vehicle = new Vehicle
{
    Id = Guid.NewGuid(),
    UserId = user.Id,  // Store only the ID, no need to set User reference
    VehicleTypeId = VehicleTypeEnum.SmallCar, // Just set the ID
    Vin = "1HGCM82633A123456",
    Plate = "ABC123",
    Make = "Toyota",
    Model = "Corolla",
    Year = 2020,
    RegisteredDate = DateTime.UtcNow
};
```
🚀 **Now, it works without errors!** You only fetch `VehicleType` when needed, keeping the object creation simpler.

---

### **Summary of Fixes**
✅ Store only `VehicleTypeId`, not `VehicleType` reference.
✅ `User` is nullable (`User? User { get; set; }`), so you only provide `UserId`.
✅ When fetching `VehicleType`, use `_context.VehicleTypes.FindAsync((byte)enumValue)`.

Would you like an example query for retrieving a vehicle with its `VehicleType`? 🚗