# Lookup Entities and Mappings

## Question

1. Do lookup entities need their own repositories?
2. Should we also define enum types? If so how can we match the enum values with the table values? Do we have to do it manually or is there some way to automate it?
3. How about link tables for many-to-many relationships? Do we need entities for them, or just mappings? IMO the latter is better, so should the mapping happen when mapping from entities to DTOs?

---

## Generated Answer

### **📌 1. Do Lookup Entities Need Their Own Repositories?**
**Short Answer: Not necessarily, but it depends on how dynamic they are.**

- If lookup tables (e.g., `UserRole`, `VehicleType`) **never change** and are just used for reference, **you don’t need separate repositories**. Instead, you can fetch them via **EF Core’s `DbContext`**.
- If you allow admins to **add, remove, or modify** lookup values dynamically (e.g., adding new vehicle types), **then a repository makes sense**.

#### **🚀 When to Use a Repository for Lookup Tables**
| **Scenario**                      | **Need Repository?** | **Why?** |
|------------------------------------|----------------------|----------|
| Static lookup data (e.g., `UserRole` is predefined and never changes) | ❌ No | Data is fixed; can hardcode or fetch directly. |
| Lookup data that changes (e.g., `VehicleType` is customizable) | ✅ Yes | You need CRUD operations. |
| Large lookup tables with frequent reads | ✅ Yes | Optimized query logic may be needed. |

---
### **📌 2. Should We Also Define Enum Types?**
Yes, defining **enums** alongside lookup tables can be useful to provide **strongly typed references** in code.

#### **🚀 How to Match Enum Values with Table Values?**
There are two main approaches:

#### **1️⃣ Store Enum Values in Lookup Tables**
- Enum values are stored in the database and mapped automatically.

```csharp
public enum UserRoleEnum
{
    Admin = 1,
    Driver = 2,
    Passenger = 3
}

public class UserRole : LookupEntity
{
    public UserRoleEnum RoleType { get; set; }
}
```

**How to Use?**
```csharp
UserRole adminRole = new UserRole { Id = (int)UserRoleEnum.Admin, Name = "Admin", RoleType = UserRoleEnum.Admin };
```
✅ **Benefit:** Strong type safety and easy to map.

---
#### **2️⃣ Enum ↔ Lookup Table Mapping via Extension Methods**
- You don’t store the enum directly, but **map it manually**.

```csharp
public static class UserRoleMapping
{
    private static readonly Dictionary<int, UserRoleEnum> _roleMap = new()
    {
        { 1, UserRoleEnum.Admin },
        { 2, UserRoleEnum.Driver },
        { 3, UserRoleEnum.Passenger }
    };

    public static UserRoleEnum ToEnum(this int roleId)
    {
        return _roleMap.TryGetValue(roleId, out var roleEnum) ? roleEnum : throw new ArgumentException("Invalid role ID");
    }

    public static int ToId(this UserRoleEnum roleEnum)
    {
        return _roleMap.First(x => x.Value == roleEnum).Key;
    }
}
```

**How to Use?**
```csharp
int roleId = 2;
UserRoleEnum roleEnum = roleId.ToEnum(); // Converts int → Enum
int dbRoleId = UserRoleEnum.Driver.ToId(); // Converts Enum → int
```
✅ **Benefit:** Works well when lookup tables are dynamic.

---
### **📌 3. How About Many-to-Many Link Tables?**
**Short Answer: Use EF Core Mappings Instead of Separate Entities (When Possible).**

If the **link table has no extra fields**, you **don’t need an entity**—just use EF Core **fluent API mapping**.

#### **1️⃣ Many-to-Many Without Extra Fields (No Separate Entity)**
```csharp
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<Role> Roles { get; set; } = new List<Role>();
}

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<User> Users { get; set; } = new List<User>();
}
```

**Fluent API Mapping:**
```csharp
modelBuilder.Entity<User>()
    .HasMany(u => u.Roles)
    .WithMany(r => r.Users)
    .UsingEntity(j => j.ToTable("UserRoles"));
```
✅ **EF Core automatically creates the `UserRoles` link table.**

---
#### **2️⃣ Many-to-Many With Extra Fields (Separate Entity Needed)**
If you need extra data (e.g., `AssignedAt` timestamp), **then create an explicit entity**.

```csharp
public class UserRole
{
    public int UserId { get; set; }
    public User User { get; set; }

    public int RoleId { get; set; }
    public Role Role { get; set; }

    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
}
```
✅ **Best When:** You need extra fields (e.g., timestamps, metadata).

---
### **📌 4. Where Should the Mapping Happen?**
- **EF Core Mapping:** Happens **inside the Infrastructure layer** (DAL).
- **DTO Mapping:** Happens **inside the Application layer** (BLL).
- **Conversion Between DTO & Entity:** Use **AutoMapper** in the Application layer.

---
### **🚀 Final Takeaways**
✅ **Lookup Tables:**
   - No repository needed for static data.
   - Use repositories for dynamic data.
   - Can store enums in DB or map manually.

✅ **Enum ↔ Table Mapping:**
   - Store in DB with direct mapping.
   - Use an extension method for manual mapping.

✅ **Many-to-Many Tables:**
   - If **no extra fields**, use **EF Core Fluent API**.
   - If **extra fields needed**, create a separate **link entity**.

Would you like an **AutoMapper example** for DTOs? 🚀