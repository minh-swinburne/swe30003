# Dependency Injection & Combination of Service Layer Pattern and MediatR

## Question

Will the `DependencyInjection` lie in Application or Presentation layer? if the former then how will the `builder.Services.AddApplication();` detect the services? and another important thing, can I use the Service Layer pattern *together* with MediatR (mediator design pattern) which uses handlers, or can I only use one of them?

---

## Generated Answer

### **📌 Where Should the `DependencyInjection` Class Be?**
The `DependencyInjection` class **should be in the Application Layer**, because:

- The Application Layer **defines service interfaces** (e.g., `IUserService`) and **implements them**.
- It **knows about repositories** (`ICosmosRepository<T>`), but **not their implementation** (`CosmosRepository<T>` in Infrastructure).
- The **Presentation Layer should only call the Application Layer**, not directly reference Infrastructure.

---

### **📌 How Will `builder.Services.AddApplication();` Detect the Services?**
To allow the **Web API project** to detect Application Services, we **add the Application project as a reference** and use `Microsoft.Extensions.DependencyInjection` in `DependencyInjection.cs`.

#### **✅ Step 1: Create `DependencyInjection.cs` in the Application Layer**
```csharp
using Microsoft.Extensions.DependencyInjection;
using MyProject.Application.Services; // Namespace for services
using MyProject.Domain.Interfaces;   // Namespace for repository interfaces
using MyProject.Infrastructure.Repositories; // Only if needed

namespace MyProject.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Register Application Services
            services.AddScoped<IUserService, UserService>();

            // Register MediatR (if used)
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

            return services;
        }
    }
}
```
---

#### **✅ Step 2: Register `AddApplication()` in Web API (`Program.cs`)**
In the **Web API project**, call the `AddApplication()` method:
```csharp
using MyProject.Application;

var builder = WebApplication.CreateBuilder(args);

// Register Application Services
builder.Services.AddApplication();

var app = builder.Build();
```
✅ Now **Web API does not directly reference Infrastructure**, it only depends on **Application Layer**.

---

## **📌 Can I Use Service Layer with MediatR (CQRS)?**
Yes, you **can use both the Service Layer Pattern and MediatR together** in a clean architecture.

📌 **How Do They Work Together?**
- **Service Layer (`IUserService`)**:
  - Good for grouping **business logic that does not fit into CQRS**.
  - Can internally use MediatR to **send commands/queries**.
- **MediatR (`IMediator.Send()`)**:
  - Used for **CQRS-style Command & Query separation**.
  - Replaces direct method calls with a **loosely coupled request-response pattern**.

---

## **📌 How to Combine Service Layer with MediatR?**
### **✅ Approach 1: Service Layer Uses MediatR**
📌 In this approach, `IUserService` calls `IMediator.Send()`, delegating logic to **Handlers**.

#### **🔹 1. Define Request (`GetUserQuery.cs`)**
```csharp
using MediatR;
using MyProject.Domain.Entities;

public record GetUserQuery(string UserId) : IRequest<User>;
```

#### **🔹 2. Define Handler (`GetUserQueryHandler.cs`)**
```csharp
using MediatR;
using MyProject.Domain.Interfaces; // ICosmosRepository<T>

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, User>
{
    private readonly ICosmosRepository<User> _repository;

    public GetUserQueryHandler(ICosmosRepository<User> repository)
    {
        _repository = repository;
    }

    public async Task<User> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetByIdAsync(request.UserId, cancellationToken: cancellationToken);
    }
}
```

#### **🔹 3. Modify `IUserService` to Use `IMediator`**
```csharp
using MediatR;

public class UserService : IUserService
{
    private readonly IMediator _mediator;

    public UserService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<User> GetUserByIdAsync(string userId)
    {
        return await _mediator.Send(new GetUserQuery(userId));
    }
}
```
🔹 Now, `IUserService` **acts as a facade** while using MediatR internally.

✅ **Advantages:**
- **Existing business logic stays inside services.**
- **MediatR is used for CQRS but doesn’t replace services.**
- **Controllers only call `IUserService`, not `IMediator`.**

---

## **📌 Alternative: Remove Service Layer, Use MediatR Only**
Instead of `IUserService`, we directly inject `IMediator` into controllers.

#### **🔹 1. Controller Uses MediatR**
```csharp
[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(string id)
    {
        var user = await _mediator.Send(new GetUserQuery(id));
        return user != null ? Ok(user) : NotFound();
    }
}
```
✅ **Advantage**: No need for `IUserService`.
❌ **Disadvantage**: If business logic **does not fit CQRS**, you may need to create new Queries/Commands for everything.

---

## **📌 Conclusion**
| ✅ **Approach** | 🔹 **Use Case** |
|---------------|----------------|
| **Service Layer + MediatR** | Best for apps where some logic needs CQRS but not all. The Service Layer acts as a "facade." |
| **MediatR Only** | Best for **pure CQRS** applications. Every action (read/write) must go through a `Query` or `Command`. |

🔹 If your app already has a **Service Layer**, then **keep it and integrate MediatR inside** (Approach 1).
🔹 If you're **fully CQRS-focused**, then **replace Service Layer with MediatR** (Approach 2).
