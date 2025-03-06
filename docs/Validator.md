# Validators with FluentValidation

## Question

now I want to add validators using FluentValidation to the system to help with data validation. Is it true that this will help relieve the burden of validation from the entity classes (so that we can let public fields instead of private fields with get-set methods)? will and should the validation happen before the request reach service / handler? how can I register their behaviour so that the validators are automatically mapped? and what would the workflow be now?

---

## Answer

## **📌 Integrating FluentValidation in the System**
Yes, **FluentValidation** helps **separate validation logic** from entity classes, which means:
✅ **Entities remain clean** (no more private fields with `get/set` validation).
✅ **Validation can be centralized** and **automatically applied** before reaching the service or handler.

---

## **📌 Where Should Validation Happen?**
FluentValidation can validate data:
1. **Before reaching the Service/Handler**: 🏆 **Best Practice!**
   - Handled via **Pipeline Behavior (MediatR)** → Automatically runs before commands.
   - Ensures **invalid requests never reach the business logic**.
   - Avoids unnecessary database operations for invalid data.

2. **Inside the Handler/Service** (❌ Not Recommended)
   - Requires manually calling `.Validate()` inside each handler/service.
   - **Duplicated effort** and **harder to maintain**.

---

## **📌 Steps to Integrate FluentValidation in MediatR (Best Practice)**

### **🔹 1. Install FluentValidation Package**
Run the following command in your terminal:
```sh
dotnet add package FluentValidation.AspNetCore
dotnet add package FluentValidation.DependencyInjectionExtensions
```

---

### **🔹 2. Create a Validator (`BookRideCommandValidator.cs`)**
Instead of putting validation logic in `BookRideCommandHandler`, create a **separate validator**:

```csharp
using FluentValidation;

public class BookRideCommandValidator : AbstractValidator<BookRideCommand>
{
    public BookRideCommandValidator()
    {
        RuleFor(x => x.PassengerId)
            .NotEmpty().WithMessage("Passenger ID is required");

        RuleFor(x => x.PickupLocation)
            .NotEmpty().WithMessage("Pickup location is required")
            .MaximumLength(100).WithMessage("Pickup location is too long");

        RuleFor(x => x.DropoffLocation)
            .NotEmpty().WithMessage("Dropoff location is required")
            .MaximumLength(100).WithMessage("Dropoff location is too long");
    }
}
```

✅ **No need for validation logic inside `BookRideCommandHandler` anymore!**

---

### **🔹 3. Add a Validation Pipeline to MediatR**
To **automatically apply FluentValidation before a request reaches the handler**, we need to create a **MediatR Pipeline Behavior**:

```csharp
using FluentValidation;
using MediatR;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);
        var failures = _validators
            .Select(v => v.Validate(context))
            .SelectMany(result => result.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Any())
        {
            throw new ValidationException(failures);
        }

        return await next(); // Proceed to the handler if validation passes
    }
}
```

✅ **This ensures that every MediatR request gets validated before execution!**

---

### **🔹 4. Register FluentValidation & MediatR in DI (`Program.cs`)**
In your **Web API project**, update `Program.cs`:

```csharp
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MyProject.Application.Commands; // Import BookRideCommand
using MyProject.Application.Validators; // Import BookRideCommandValidator

var builder = WebApplication.CreateBuilder(args);

// 🔹 Register MediatR (Scan all handlers)
builder.Services.AddMediatR(typeof(BookRideCommand).Assembly);

// 🔹 Register FluentValidation (Scan all validators)
builder.Services.AddValidatorsFromAssembly(typeof(BookRideCommandValidator).Assembly);

// 🔹 Add Validation Pipeline to MediatR
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

var app = builder.Build();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.Run();
```

✅ **Now, all MediatR requests will be automatically validated before execution!**

---

## **📌 Updated Workflow with FluentValidation**
1. **Client sends an API request** (e.g., `POST /api/rides/book`).
2. **Controller sends the request to MediatR** (`IMediator.Send(new BookRideCommand(...))`).
3. **ValidationBehavior runs first** (before the command handler).
4. ✅ If the request is valid → MediatR calls `BookRideCommandHandler`.
   ❌ If invalid → MediatR throws `ValidationException` (controller returns `400 Bad Request`).
5. **Handler processes the request** and calls the service layer.

---

## **📌 Why Is This Better?**
| ✅ **With FluentValidation + MediatR Pipeline** | ❌ **Without FluentValidation Pipeline** |
|-----------------------------|--------------------------------|
| **Validation runs automatically** before handlers. | **Handlers manually call `.Validate()`**. |
| **Keeps handlers/services clean** and focused on business logic. | **Validation logic pollutes handlers**. |
| **Easier to maintain and test** validation separately. | **Requires writing validation repeatedly** in multiple places. |

---

## **📌 Conclusion**
🚀 **With FluentValidation + MediatR Pipeline:**
- **Validation happens before reaching the service/handler** (best practice ✅).
- **Business logic is separate from validation**.
- **No need for manual validation calls** in handlers or services.
- **Auto-mapping of validators** ensures no missing validations.

Would you like me to extend this with **custom error handling for API responses**? 🔥
