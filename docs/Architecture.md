# System Architecture

There are multiple naming conventions for layers in an **N-Tier architecture**. The key is to choose **clear, maintainable names** while ensuring that responsibilities are well-defined.

---

## 📌 **Standard N-Tier Architecture (with Naming Conventions)**

Here’s a **well-structured breakdown** of layers with their **roles and naming conventions**:

```
/MyAppSolution
  ├── 📂 Presentation/ (UI & API Layer)
  │    ├── WebAPI/        (ASP.NET Core Web API)
  │    ├── MobileApp/     (.NET MAUI App)
  │
  ├── 📂 Application/ (Business Logic Layer, CQRS, MediatR, Services)
  │    ├── Commands/      (CQRS Commands)
  │    ├── Queries/       (CQRS Queries)
  │    ├── Handlers/      (MediatR Handlers)
  │    ├── Validators/    (FluentValidation)
  │    ├── Interfaces/    (Service Interfaces)
  │
  ├── 📂 Domain/ (Entities, DTOs, Interfaces, Enums)
  │    ├── Entities/      (Database entities)
  │    ├── DTOs/          (Request & Response DTOs)
  │    ├── Enums/         (Application-wide enums)
  │    ├── Interfaces/    (Repository & Service interfaces)
  │
  ├── 📂 Infrastructure/ (DAL, Repositories, External APIs, DB Context)
  │    ├── Persistence/   (EF Core, Dapper, NHibernate)
  │    ├── Repositories/  (Repository implementations)
  │    ├── ExternalAPIs/  (Integrations with 3rd-party services)
  │
  ├── 📂 Common/ (Constants, Exceptions, Logging, Helpers)
  │    ├── Exceptions/    (Custom exceptions)
  │    ├── Helpers/       (Utility functions)
  │    ├── Constants/     (Exception codes, messages)
```

---

## 📌 **Layer Responsibilities & Naming Guide**

### **1️⃣ Presentation Layer (UI & API)**

**📍 Folders:**`WebAPI/`, `MobileApp/`

**📍 Naming:**`"API" for Web API, `"UI"` for desktop/mobile apps

💡 **Responsibilities:**

✔ Provides UI (e.g., .NET MAUI) or API endpoints (e.g., ASP.NET Web API)

✔ **Should not contain business logic** (calls the Application layer instead)

✔ Calls MediatR to send commands/queries

📌 **Example Web API Controller**

```csharp
[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator=mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
    {
        var userId= await _mediator.Send(command);
        return CreatedAtAction(nameof(GetUser), new { id = userId }, null);
    }
}
```

---

### **2️⃣ Application Layer (Business Logic, CQRS, Services)**

**📍 Folder:**`Application/`

**📍 Naming:**`"Business"`, `"Service"`, or `"Application"` (commonly used)

💡 **Responsibilities:**

✔ **Contains business rules and logic**

✔ Uses **MediatR** for CQRS (Commands & Queries)

✔ Calls **Repositories** from the Infrastructure layer

✔ **Does not access the database directly**

📌 **Example MediatR Command & Handler**

```csharp
public record CreateUserCommand(string Name, string Email) : IRequest<int>;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, int>
{
    private readonly IUserRepository _userRepository;

    public CreateUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User { Name = request.Name, Email = request.Email };
        await _userRepository.AddUserAsync(user);
        return user.Id;
    }
}
```

---

### **3️⃣ Domain Layer (Entities, DTOs, Interfaces, Enums)**

**📍 Folder:**`Domain/`

**📍 Naming:**`"Domain"`, `"Shared"`, `"Common"`

💡 **Responsibilities:**

✔ Defines **entities** (database models)

✔ Defines **DTOs** (request/response objects)

✔ Defines **interfaces** (Repository & Service contracts)

✔ Contains **Enums** (application-wide constants)

📌 **Example Entity (Database Model)**

```csharp
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}
```

📌 **Example DTO (Data Transfer Object)**

```csharp
public class UserDto
{
    public string Name { get; set; }
    public string Email { get; set; }
}
```

📌 **Example Repository Interface**

```csharp
public interface IUserRepository
{
    Task AddUserAsync(User user);
    Task<User?> GetUserByIdAsync(int id);
}
```

---

### **4️⃣ Infrastructure Layer (DAL, Repositories, External APIs)**

**📍 Folder:**`Infrastructure/`

**📍 Naming:**`"Infrastructure"` (best practice), sometimes `"DataAccess"`

💡 **Responsibilities:**

✔ **Implements repositories** (EF Core, Dapper, NHibernate)

✔ Manages **DB Context** (EF Core, MongoDB, etc.)

✔ Manages **external API calls** (e.g., payment gateways, email services)

📌 **Example Repository Implementation**

```csharp
public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context=context;
    }

    public async Task AddUserAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }
}
```

---

### **5️⃣ Common Layer (Constants, Exceptions, Helpers)**

**📍 Folder:**`Common/`

**📍 Naming:**`"Common"`, `"Shared"`, `"Core"` (depends on scope)

💡 **Responsibilities:**

✔ Contains **custom exceptions**

✔ Contains **constants, messages**

✔ Utility **helper classes** (e.g., date formatting, JWT helpers)

📌 **Example Custom Exception**

```csharp
public class UserNotFoundException : Exception
{
    public UserNotFoundException(string message) : base(message) { }
}

```

📌 **Example Constants**

```csharp
public static class ErrorMessages
{
    public const string UserNotFound = "User not found.";
}

```

---

## 📌 **Where Should You Put Everything?**

| Component                      | Recommended Folder             |
| ------------------------------ | ------------------------------ |
| API Controllers                | `Presentation/WebAPI/`         |
| UI Pages (Maui)                | `Presentation/MobileApp/`      |
| Services (Business Logic)      | `Application/`                 |
| CQRS Commands & Queries        | `Application/`                 |
| MediatR Handlers               | `Application/Handlers/`        |
| FluentValidation Validators    | `Application/Validators/`      |
| DTOs (Request/Response Models) | `Domain/DTOs/`                 |
| Database Models (Entities)     | `Domain/Entities/`             |
| Enums                          | `Domain/Enums/`                |
| Repository Interfaces          | `Domain/Interfaces/`           |
| Repository Implementations     | `Infrastructure/Repositories/` |
| External API Calls             | `Infrastructure/ExternalAPIs/` |
| DB Context (EF Core)           | `Infrastructure/Persistence/`  |
| Constants & Helpers            | `Common/`                      |
| Custom Exceptions              | `Common/Exceptions/`           |

---

## **📌 Final Thoughts**

- Your structure **ensures separation of concerns** ✅

-**Each layer has a single responsibility** ✅

-**Web API & .NET MAUI are fully independent** ✅

---

Your dependency structure **partially follows Clean Architecture**, but there's a critical **violation of the Dependency Rule**:

### **🚨 Issue: Application Layer Should NOT Depend on Infrastructure**

Your dependency list shows **Application -> Infrastructure**, which is incorrect in Clean Architecture.

📌 **Correct dependency flow should be:**

- **Presentation** → Application
- **Application** → Domain
- **Infrastructure** → Domain
- **Infrastructure** (Repositories) → Application (**Only via Dependency Injection, not direct reference!**)
- **Common (Shared Code)** → Used by all layers (but should not introduce dependencies between layers).

---

## **✅ How to Fix: Invert the Repository Dependency**

Currently, **IRepository** is defined inside `Infrastructure`. This forces `Application` to reference `Infrastructure`, which is **wrong**.

### **Fix: Move `IRepository<T>` to the Domain Layer**

✅ In **Clean Architecture**, repository interfaces are defined in the **Domain Layer**, while their concrete implementations stay in **Infrastructure**.

📌 **Move `IRepository<T>` to `SmartRide.Domain.Interfaces.Repositories`:**

```csharp
namespace SmartRide.Domain.Interfaces.Repositories;

public interface IRepository<T> where T : Entity
{
    IQueryable<T> Query(CancellationToken cancellationToken = default);
    Task<int> CountAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string id, CancellationToken cancellationToken = default);
    Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);
    Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task<T> DeleteAsync(string id, CancellationToken cancellationToken = default);
    Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetByFilterAsync<TDto>(
        Expression<Func<T, bool>> filter,
        Expression<Func<T, TDto>>? select = null,
        Expression<Func<T, object>>? orderBy = null,
        bool ascending = true,
        int skip = 0,
        int limit = 0,
        CancellationToken cancellationToken = default
    );
}
```

### **Then, Implement the Repository in Infrastructure:**

```csharp
using SmartRide.Domain.Interfaces.Repositories;
using SmartRide.Domain.Entities.Base;

namespace SmartRide.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : Entity
{
    private readonly SmartRideDbContext _context;

    public Repository(SmartRideDbContext context)
    {
        _context = context;
    }

    public async Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        _context.Set<T>().Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    // Implement other methods...
}
```

### **🚀 Now, in the Application Layer, you depend only on `IRepository<T>`**

This way:

- **Application Layer does NOT reference Infrastructure**.
- **Infrastructure depends on Application only at runtime (via Dependency Injection)**.

---

## **✅ Final Dependency Flow (Correct)**

✅ **Your fixed dependencies follow Clean Architecture:**

```
+---------------------+
|  Presentation/UI    |  (ASP.NET API, MAUI, Console)
+---------------------+
        ↓
+---------------------+
|  Application Layer  |  (Use Cases, Services, MediatR Handlers)
+---------------------+
        ↓
+---------------------+
|  Domain Layer       |  (Entities, Enums, IRepository)
+---------------------+
        ↓
+---------------------+
|  Infrastructure     |  (EF Core, Repositories, DB, APIs)
+---------------------+
```

### **Dependency List (Fixed)**

- ✅ **Presentation** → Application
- ✅ **Application** → Domain
- ✅ **Infrastructure** → Domain
- ✅ **Infrastructure (Repositories)** → Application (only via Dependency Injection, NOT direct reference!)

---

## **🎯 Is This Still N-Tier Architecture?**

Yes! **N-Tier Architecture** and **Clean Architecture** overlap, but Clean Architecture introduces stricter separation of concerns.

- In **N-Tier**, Infrastructure (Data Access) is **always referenced by Application**, which is why your original structure allowed `Application -> Infrastructure`.
- In **Clean Architecture**, Infrastructure is **ONLY injected at runtime** to avoid tight coupling.

🚀 **By fixing this dependency issue, your project now follows both N-Tier and Clean Architecture correctly.**

Would you like an example of **Dependency Injection setup in `Program.cs` to wire everything properly?** 💡

# Question

Now I'm a bit uncertain about the Presentation Layer, like it currently has 2 different executable programs: Console app and Web API. while the console app does provide the user with a UI, it doesn't have something like a controller to forward requests yet (should this be implemented or just call directly the services?) and also needs numbered menus (perhaps using Template design pattern). Whereas, the Web API has the controllers but doesn't really provide a UI (it's gotta be a web ui project or some or FE project pointing requests to it). So are they considered Presentation Layer? Or the controllers actually belong to Application layer?

---

# Generated Answer

Your **Console App and Web API** are both part of the **Presentation Layer**, but they serve different purposes within that layer. Let’s break this down:

---

## **1️⃣ What is the Presentation Layer?**

The **Presentation Layer** is responsible for:
✅ Handling user interaction (UI or API calls).
✅ Forwarding requests to the **Application Layer** (Use Cases or Services).
✅ Returning responses to the user (e.g., displaying results or returning JSON).

It does **not** contain business logic or data access code.

---

## **2️⃣ How Do Your Projects Fit?**

### **🔹 Web API (Presentation Layer)**

- The **Controllers** in the Web API **are part of the Presentation Layer**, because:
  - They **accept HTTP requests** and **return responses** (usually JSON).
  - They **do not contain business logic** but instead call **Application Services** (Use Cases).
  - The actual UI is **separate** (e.g., a web frontend like Vue, React, or Angular).

📌 **Controllers DO NOT belong to the Application Layer**—they just **forward requests to Application Services**.

---

### **🔹 Console App (Presentation Layer)**

- The Console App also belongs to the **Presentation Layer**, but it has no controllers.
- Instead of an API, it uses **menus and direct service calls**.
- A good approach is to structure it with:
  - A **menu system** (possibly using the **Template Method pattern** to organize different screens/flows).
  - A **Command Handler** or a lightweight "Controller-like" class to manage user inputs.

📌 The Console App **still follows Clean Architecture**, just without HTTP controllers.

---

## **3️⃣ Should the Console App Have a Controller?**

- A **traditional Web API needs controllers** because it processes external HTTP requests.
- A **Console App doesn’t necessarily need controllers**, but it should still follow **separation of concerns**.
- Instead of controllers, you can structure it using:
  - A **MenuManager** class that processes input and calls **Application Services**.
  - A **Command Pattern** or **Template Method Pattern** to handle different actions.

---

## **4️⃣ Final Answer**

### ✅ Both Web API (Controllers) and Console App (Menu/UI) are part of the **Presentation Layer**.

### ❌ Controllers are **not** part of the Application Layer.

### 🎯 Your structure is correct:

- Web API -> Uses **Controllers** (Presentation Layer) to forward requests to **Application Services**.
- Console App -> Uses **Menu/UI logic** (Presentation Layer) to call **Application Services**.

Would you like an example on how to structure the **Console App menu system** efficiently? 🚀
