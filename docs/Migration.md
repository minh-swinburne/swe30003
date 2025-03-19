# Terminal Command

## Add New Migration

```
dotnet ef migrations add InitialCreate --project SmartRide.Infrastructure --startup-project SmartRide.ConsoleApp
```

## Remove Migration

```
dotnet ef migrations remove --project SmartRide.Infrastructure --startup-project SmartRide.ConsoleApp
```

## Apply Migrations into Database

```
dotnet ef database update --project SmartRide.Infrastructure --startup-project SmartRide.ConsoleApp
```

## Question

Alright, now I want to ensure that every time the `SmartRideDbContext` is created, it's synchronized with the actual database, aka migration. Should I have this done automatically, perhaps programmatically and independently of the current presentation layer, or should I do it only when needed (run command in the terminal)?

---

## Generated Answer

### **Ensuring Database Migration Sync in `SmartRideDbContext`**

There are **two main approaches** to handling migrations in your system:

1. **Automatically apply migrations programmatically** *(every time the app starts)*
2. **Manually apply migrations using EF Core CLI** *(only when needed)*

Both have pros and cons, depending on your environment and deployment strategy.

---

## **1️⃣ Automatic Migrations on Startup (Programmatic)**

If you want to ensure that the `SmartRideDbContext` is always synchronized with the database automatically, you can **apply migrations programmatically** when the application starts.

### **✅ Pros**

- Ensures **the latest schema** is always applied, reducing manual intervention.
- Useful for **small projects** or **internal applications** where DB schema changes are frequent.
- Works well in **development or staging environments**.

### **❌ Cons**

- **Risky in production**: Applying migrations automatically in production can lead to downtime or unexpected issues.
- Doesn't handle **rollback** scenarios cleanly.
- May cause **delays on startup** if the migration takes time.

### **📌 How to Implement**

Modify your `Program.cs` (for both the console app and Web API):

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmartRide.Infrastructure.Persistence;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddInfrastructure(context.Configuration);
    })
    .Build();

// Apply Migrations Automatically
using (var scope = host.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SmartRideDbContext>();
    dbContext.Database.Migrate();
}

host.Run();
```

### **📌 Explanation**

- The `dbContext.Database.Migrate();` call **automatically applies any pending migrations** on startup.
- Uses **dependency injection** (`CreateScope()`) to retrieve the `SmartRideDbContext`.
- This approach ensures the database is **always up to date**.

---

## **2️⃣ Manual Migrations via EF Core CLI**

If you want more **control over database updates**, it's better to run migrations **only when needed**.

### **✅ Pros**

- **Safer for production**: You can review schema changes before applying them.
- Allows for **better control** over rollback strategies.
- Recommended for **large-scale applications** with multiple environments (dev, test, production).

### **❌ Cons**

- Requires **manual execution** every time you make a schema change.
- Developers need to **remember to apply migrations** before running the app.

### **📌 How to Apply Migrations Manually**

1. **Install dotnet-ef Tool Globally**
   Run the following command in your terminal to install the dotnet-ef tool globally:

```sh
  dotnet tool install --global dotnet-ef
```

2. **Generate a new migration** (after making changes to entity models)
   ```sh
   dotnet ef migrations add AddNewColumnToUsers --project SmartRide.Infrastructure
   ```
3. **Apply the migration to the database**
   ```sh
   dotnet ef database update --project SmartRide.Infrastructure
   ```
4. **Verify the migration**
   - Check the `__EFMigrationsHistory` table in your database.

---

## **🚀 Best Practice: Hybrid Approach**

For **development and staging**, enable **automatic migrations**.
For **production**, use **manual migrations**.

### **How to Do This?**

Modify your program so that migrations run **only in development mode**:

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmartRide.Infrastructure.Persistence;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddInfrastructure(context.Configuration);
    })
    .Build();

using (var scope = host.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SmartRideDbContext>();

    var env = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();
    if (env.IsDevelopment()) // Run automatic migration only in Development
    {
        dbContext.Database.Migrate();
    }
}

host.Run();
```

This ensures:

- **Automatic migrations in development** (so developers don’t need to manually update the DB).
- **Manual migrations in production**, avoiding unexpected schema changes.

---

## **🔥 Conclusion**

- **For local development?** ✅ Enable automatic migrations.
- **For production?** ❌ Use manual migrations.
- **Hybrid approach?** 🚀 Automatically apply migrations only in **development**.

This ensures your app stays **in sync with the database** while maintaining **production stability**.

Do you need help with setting up migration scripts for deployment? 🚀
