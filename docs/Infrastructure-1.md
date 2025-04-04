# Fluent API Mapping and EF Core

## Question

where will the Fluent API Mapping be defined (probably in the Infrastructure layer but how)? and also I read that EF Core's DbSet is actually an implementation of the Repository pattern. So is there still any meaning in using a custom Repository with interface like this:

```
using SmartRide.Domain.Entities.Base;
using System.Linq.Expressions;

namespace SmartRide.Infrastructure.Repositories;

public interface IRepository<T> where T : Entity
{
    IQueryable<T> Query(CancellationToken cancellationToken = default);
    Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);
    Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task<T> DeleteAsync(string id, CancellationToken cancellationToken = default);
    Task<T> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetByFilterAsync(Expression<Func<T, bool>> filter, Expression<Func<T, bool>>? select, Expression<Func<T, bool>>? orderBy = null, bool ascending = true, int limit = 0, CancellationToken cancellationToken = default);
}
```

or should we switch to DbSet? Or can we combine?

---

## Generate Answer

### **📌 1. Where Should Fluent API Mapping Be Defined?**

You're correct that **Fluent API mappings** belong in the **Infrastructure Layer** because they deal with database configurations.

#### **🚀 How to Define Fluent API Mappings?**

- They should be placed inside `Infrastructure/Persistence/Configurations/` (or similar).
- Use **EF Core's `IEntityTypeConfiguration<T>`** to configure entity relationships.

#### **✅ Example: Mapping `User` ↔ `Role` (Many-to-Many)**

Create a configuration class:

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartRide.Domain.Entities.Base;

namespace SmartRide.Infrastructure.Persistence.Configurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasKey(ur => new { ur.UserId, ur.RoleId }); // Composite primary key

        builder.HasOne(ur => ur.User)
               .WithMany(u => u.UserRoles)
               .HasForeignKey(ur => ur.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ur => ur.Role)
               .WithMany(r => r.UserRoles)
               .HasForeignKey(ur => ur.RoleId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.Property(ur => ur.AssignedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}
```

Then, apply this configuration inside `DbContext`:

```csharp
using Microsoft.EntityFrameworkCore;
using SmartRide.Domain.Entities.Base;

namespace SmartRide.Infrastructure.Persistence;

public class SmartRideDbContext : DbContext
{
    public SmartRideDbContext(DbContextOptions<SmartRideDbContext> options) : base(options) {}

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; } // Explicit entity for many-to-many

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}
```

✅ **Now EF Core knows how to map the `UserRole` table automatically!**

---

### **📌 2. Is a Custom Repository Meaningful If EF Core’s `DbSet` is Already a Repository?**

You're absolutely right that **EF Core’s `DbSet<T>` is already an implementation of the Repository Pattern**. However, whether to **still use a custom repository pattern** depends on your project's complexity.

#### **✅ When to Use Only `DbSet<T>` (No Custom Repositories)**

- If your project is simple and doesn’t require **custom business logic** for queries.
- When using **CQRS** (Command Query Responsibility Segregation) with **MediatR** (because handlers already handle DB access).
- Example:
  ```csharp
  var users = await _dbContext.Users.ToListAsync();
  ```

---

#### **✅ When to Use a Custom Repository (Like Your `IRepository<T>` Interface)**

- If you want to **encapsulate complex queries** or **abstract** the database technology (e.g., switching from EF Core to Dapper or another ORM in the future).
- If you need **custom filtering, caching, or logging** for repository calls.
- If you use **multiple databases (e.g., MySQL + MongoDB)** and need a common interface.

---

### **📌 3. Can We Combine Custom Repository with `DbSet`?**

Yes! You can **wrap `DbSet<T>` inside a repository implementation** while still benefiting from EF Core.

#### **✅ Hybrid Approach Example**

```csharp
public class Repository<T> : IRepository<T> where T : Entity
{
    private readonly SmartRideDbContext _dbContext;
    private readonly DbSet<T> _dbSet;

    public Repository(SmartRideDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = dbContext.Set<T>();
    }

    public IQueryable<T> Query(CancellationToken cancellationToken = default)
    {
        return _dbSet.AsQueryable();
    }

    public async Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        var result = await _dbSet.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return result.Entity;
    }

    public async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Update(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<T> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbSet.FindAsync(id, cancellationToken);
        if (entity == null) throw new KeyNotFoundException("Entity not found");

        _dbSet.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<T> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.ToListAsync(cancellationToken);
    }
}
```

Then, register it in **DI Container** inside the Web API project:

```csharp
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
```

✅ **Now you get the best of both worlds:**

1. **Direct DbSet Usage** when you want simple queries.
2. **Custom Repository** when you need abstraction.

---

### **📌 4. Summary**

| **Approach**                                  | **When to Use?**                                         | **Pros**                           | **Cons**                             |
| --------------------------------------------- | -------------------------------------------------------- | ---------------------------------- | ------------------------------------ |
| **Direct `DbSet<T>` (No Repository)**         | Simple apps, MediatR, CQRS                               | Less boilerplate, direct DB access | No abstraction, harder to swap ORM   |
| **Custom Repository (`IRepository<T>`)**      | Complex queries, multiple DBs, encapsulation             | Encapsulates logic, easier to test | More code, possible over-engineering |
| **Hybrid Approach (Repository + `DbSet<T>`)** | Best for large apps with mix of simple & complex queries | Flexibility, can use both          | Requires careful design              |

Would you like an example of **unit testing** the repository? 🚀
