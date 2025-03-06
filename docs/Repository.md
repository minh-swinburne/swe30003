# Generic Repository vs Specific Repositories & Dependency by Presentation Layer

## Question

In a large project I see the devs use a common interface with type for repositories instead of implementing each one of them, like this:
```csharp
using System.Linq.Expressions;
using MMory.Core.Entities.Base;

namespace MMory.Core.Interfaces.Repositories;

public interface ICosmosRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(string id, string partitionKey = "", CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetItemsAsync(string queryString, CancellationToken cancellationToken = default);
    Task<T> AddAsync(T entity, string? partitionKey = "", CancellationToken cancellationToken = default);
    Task UpdateAsync(string id, T entity, string? partitionKey = "", CancellationToken cancellationToken = default);
    Task DeleteAsync(string id, string? partitionKey = "", CancellationToken cancellationToken = default);
    Task<(IEnumerable<TDto> Data, int TotalRecord)> GetItemsAsync<TDto>(Expression<Func<T, bool>> filter, Expression<Func<TDto, object>> orderBy = null, bool ascending = true, Expression<Func<T, TDto>> select = null, int skip = 0, int limit = 0);
    Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>>? filter, int skip = 0, int limit = 0);
    IQueryable<T> GetQuery();
}
```
And then they register it to the builder in the Web API project like this:
```csharp
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        _ = builder.Services.AddScoped(typeof(ICosmosRepository<>), typeof(CosmosRepository<>));
```
Is it better than the manual, particular repository interfaces? And also, the interface is defined in the "Domain" layer, which is fine, but the `CosmosRepository` is defined in the "Infrastructure" layer, making the Presentation layer depend on the Infrastructure. Is this bad, and is it compile time or runtime dependency?

---

## Generated Answer

### **📌 Generic Repository vs. Specific Repository Interfaces**
Using a **generic repository** like `ICosmosRepository<T>` is a **common best practice in large projects** because:

✅ **Reduces Code Duplication**:
   - Instead of writing separate repository interfaces (`IUserRepository`, `IProductRepository`, etc.), a single interface works for all entities.

✅ **Enforces Consistency**:
   - Every entity follows the same CRUD operations, ensuring a uniform approach to data access.

✅ **Easier Maintenance**:
   - If you need to modify a method (e.g., change the `GetItemsAsync` signature), you only update one place.

✅ **Works Well with Dependency Injection (DI)**:
   - The repository implementation can be **injected dynamically** based on the entity type.

📌 **When Should You Use Specific Repositories Instead?**
- If **business logic for data access is unique per entity** (e.g., `OrderRepository` needs special queries that `CustomerRepository` doesn't).
- If **you need separate interfaces for different data sources** (e.g., `ICosmosRepository`, `ISqlRepository`, etc.).

---

### **📌 Is It Bad That the Presentation Layer Uses the Infrastructure Layer?**
Yes, **Presentation Layer should never directly depend on Infrastructure Layer!**
This would create **tight coupling** and violate the **Dependency Inversion Principle (DIP)**.

---

### **📌 Why Is This a Problem?**
🚨 **Issue: Tight Coupling**
- If Infrastructure Layer changes (e.g., switching from CosmosDB to PostgreSQL), the Presentation Layer must also be updated.

🚨 **Issue: Harder to Test**
- If the API controller directly depends on `CosmosRepository`, mocking data access in unit tests becomes difficult.

🚨 **Issue: Violates Clean Architecture**
- In **Clean Architecture**, dependencies should always **flow inward** (Presentation → Application → Domain → Infrastructure).
- The **Presentation Layer should depend only on abstractions (interfaces), not implementations.**

---

### **📌 Compile-Time vs. Runtime Dependency**
**Compile-Time Dependency**
📌 *"Does the Presentation Layer know about Infrastructure Layer during compilation?"*
- **Yes**, because `builder.Services.AddScoped(typeof(ICosmosRepository<>), typeof(CosmosRepository<>));` is **declared in the Web API project**.

**Runtime Dependency**
📌 *"Does the Web API actually use CosmosRepository at runtime?"*
- **Yes**, because DI resolves `ICosmosRepository<T>` to `CosmosRepository<T>` when the app runs.

🚨 **Compile-Time Dependency = Bad**
- The Web API **shouldn't even reference Infrastructure** in the first place.

🚨 **Runtime Dependency = OK if done correctly**
- The Web API **should depend on the Application Layer**, which should handle repository abstraction.

---

### **📌 How to Fix This?**
Instead of injecting **Infrastructure (`CosmosRepository<>`)** directly into Web API, we **inject it into the Application Layer instead**.

---

### **✅ Fix #1: Move Dependency Injection to Application Layer**
Instead of registering `CosmosRepository` in the Web API:

#### ✅ **Move This Registration to Application Layer**
```csharp
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped(typeof(ICosmosRepository<>), typeof(CosmosRepository<>));
        return services;
    }
}
```
Now, in **Web API** (`Program.cs`), just call:

```csharp
builder.Services.AddApplication();
```
✅ **Web API now only depends on the Application Layer, not Infrastructure!**

---

### **✅ Fix #2: Use Application Services Instead of Repositories**
Instead of injecting `ICosmosRepository<T>` into the API controller, create **Application Services**.

📌 **Controller Should Call `IUserService`, Not `ICosmosRepository`**
```csharp
[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(string id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        return user != null ? Ok(user) : NotFound();
    }
}
```

---

### **📌 Final Thoughts**
✅ **Using Generic Repository (`ICosmosRepository<T>`) is a good practice for reducing duplication.**
❌ **Registering `CosmosRepository<>` in the Web API project is a bad practice because it creates direct dependency on Infrastructure.**
✅ **Fix: Move DI to Application Layer or introduce Application Services (`IUserService`).**
✅ **Presentation Layer should only depend on Application Layer, NOT Infrastructure.**

Would you like an example of **unit testing** this structure? 🚀