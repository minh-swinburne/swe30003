# Question

how can I use Bogus, which layer should I install it, where should I define the mocking data so that I can call it when I want (or maybe when creating migration) without it being called when I simply run the project

---

# Answer

To use `Bogus` effectively for mocking data, you should follow these steps:

---

### 1. **Install `Bogus` in the Appropriate Layer**
   - Install `Bogus` in the **Infrastructure** layer or a dedicated **Test/Seed** project.
   - This keeps the mocking logic separate from your core application logic.
   - Command to install:
     ```bash
     dotnet add package Bogus
     ```

---

### 2. **Define Mocking Data in a Separate Class**
   - Create a dedicated class for generating mock data. This class should reside in the **Infrastructure** layer or a dedicated **Seed** folder.
   - Example: Create a `MockDataGenerator` class.

#### Example: `MockDataGenerator.cs`
```csharp
using Bogus;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Enums;

namespace SmartRide.Infrastructure.Seed;

public static class MockDataGenerator
{
    public static List<User> GenerateUsers(int count)
    {
        var userFaker = new Faker<User>()
            .RuleFor(u => u.Id, f => Guid.NewGuid())
            .RuleFor(u => u.FirstName, f => f.Name.FirstName())
            .RuleFor(u => u.LastName, f => f.Name.LastName())
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.Phone, f => f.Phone.PhoneNumber())
            .RuleFor(u => u.Password, f => f.Internet.Password())
            .RuleFor(u => u.UserRoles, f => new List<UserRole>
            {
                new UserRole
                {
                    RoleId = f.PickRandom<RoleEnum>(),
                    UserId = Guid.NewGuid()
                }
            });

        return userFaker.Generate(count);
    }

    public static List<Vehicle> GenerateVehicles(int count, List<User> users)
    {
        var vehicleFaker = new Faker<Vehicle>()
            .RuleFor(v => v.Id, f => Guid.NewGuid())
            .RuleFor(v => v.UserId, f => f.PickRandom(users).Id)
            .RuleFor(v => v.VehicleTypeId, f => f.PickRandom<VehicleTypeEnum>())
            .RuleFor(v => v.Vin, f => f.Vehicle.Vin())
            .RuleFor(v => v.Plate, f => f.Random.String2(7, "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"))
            .RuleFor(v => v.Make, f => f.Vehicle.Manufacturer())
            .RuleFor(v => v.Model, f => f.Vehicle.Model())
            .RuleFor(v => v.Year, f => f.Date.Past(10).Year)
            .RuleFor(v => v.RegisteredDate, f => f.Date.Past(5));

        return vehicleFaker.Generate(count);
    }
}
```

---

### 3. **Call Mock Data Generation When Needed**
   - Use the `MockDataGenerator` class in a controlled way, such as during migrations or in a development-only seeding process.
   - Avoid calling it during normal application runtime.

#### Example: Seed Data During Migration
Modify your `SmartRideDbContext` to include a method for seeding mock data.

```csharp
public void SeedMockData()
{
    if (!Users.Any())
    {
        var mockUsers = MockDataGenerator.GenerateUsers(10);
        Users.AddRange(mockUsers);
    }

    if (!Vehicles.Any())
    {
        var mockVehicles = MockDataGenerator.GenerateVehicles(10, Users.ToList());
        Vehicles.AddRange(mockVehicles);
    }

    SaveChanges();
}
```

---

### 4. **Control When Mock Data is Used**
   - Call the `SeedMockData` method only in specific scenarios, such as during migrations or development.

#### Example: Call in `Program.cs` for Development Only
```csharp
using SmartRide.Infrastructure.Persistence;

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<SmartRideDbContext>();
    dbContext.SeedMockData();
}

app.Run();
```

---

### 5. **Alternative: Use a Separate Seeder for Migrations**
   - Create a separate console application or script to seed mock data into the database.
   - This keeps the seeding logic completely separate from the main application.

---

### Why This Approach Works
- **Separation of Concerns**: Mock data generation is isolated in a dedicated class.
- **Controlled Execution**: Mock data is only seeded when explicitly called (e.g., during development or migrations).
- **Flexibility**: You can easily adjust the number and type of mock data without affecting the core application logic.