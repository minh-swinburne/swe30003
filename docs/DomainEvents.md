# Question

what is the use of domain events? give some example scenarios where domain events are useful and efficient. besides, how would it work, how can it be published (what mediator, can we use mediatR), and what will subscribe to those events (how)?

---

# Answer

Reference: [Domain events: Design and implementation (Microsoft)](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/domain-events-design-implementation)

### **Use of Domain Events**
Domain events are a way to capture and communicate significant changes or actions within the domain model. They allow decoupling between different parts of the system by enabling one part to raise an event and other parts to react to it without direct dependencies. This promotes a clean architecture and adheres to the **Single Responsibility Principle**.

#### **Key Benefits**
1. **Decoupling**: The publisher of the event does not need to know about the subscribers.
2. **Scalability**: Events can be handled asynchronously, improving system performance.
3. **Auditability**: Events can be logged for tracking changes in the system.
4. **Extensibility**: New behaviors can be added by subscribing to events without modifying the existing code.

---

### **Example Scenarios**
1. **User Registration**:
   - **Event**: `UserCreatedEvent`
   - **Subscribers**:
     - Send a welcome email.
     - Create a default profile for the user.
     - Notify the admin of a new user.

2. **Payment Processing**:
   - **Event**: `PaymentCreatedEvent`
   - **Subscribers**:
     - Update the user's subscription status.
     - Generate an invoice.
     - Notify the user of successful payment.

3. **Ride Booking**:
   - **Event**: `RideCreatedEvent`
   - **Subscribers**:
     - Notify the driver of a new ride request.
     - Update the vehicle's availability status.
     - Log the ride creation for analytics.

4. **License Expiry**:
   - **Event**: `LicenseUpdatedEvent`
   - **Subscribers**:
     - Notify the user of the upcoming expiry.
     - Trigger a background job to validate the updated license.

---

### **How Domain Events Work**
1. **Raising Events**:
   - Events are raised in the domain model when a significant action occurs (e.g., in the `OnSave` method of entities like `User`, `Vehicle`, etc.).
   - Example:
     ```csharp
     AddDomainEvent(new UserCreatedEvent(this));
     ```

2. **Publishing Events**:
   - Events are published after the database transaction is committed (to ensure consistency).
   - The `SmartRideDbContext` processes entities and dispatches events:
     ```csharp
     foreach (var domainEvent in entry.Entity.DomainEvents)
     {
         // Dispatch the domain event
         Console.WriteLine($"Dispatching event: {domainEvent.GetType().Name}");
     }
     ```

3. **Subscribing to Events**:
   - Subscribers listen to specific events and execute corresponding actions.
   - Example:
     - A subscriber for `UserCreatedEvent` might send a welcome email.

---

### **Using MediatR for Domain Events**
MediatR is a lightweight library for implementing the **Mediator Pattern**. It can be used to publish and handle domain events efficiently.

#### **Steps to Use MediatR**
1. **Install MediatR**:
   ```bash
   dotnet add package MediatR
   dotnet add package MediatR.Extensions.Microsoft.DependencyInjection
   ```

2. **Define Domain Events**:
   - Example:
     ```csharp
     public record UserCreatedEvent(User User) : INotification;
     ```

3. **Publish Events**:
   - Modify `SmartRideDbContext` to use MediatR for publishing events:
     ```csharp
     private readonly IMediator _mediator;

     public SmartRideDbContext(DbContextOptions<SmartRideDbContext> options, IOptions<DbSettings> dbSettings, IMediator mediator)
         : base(options)
     {
         _mediator = mediator;
         _dbSettings = dbSettings.Value;
     }

     private async Task DispatchDomainEventsAsync()
     {
         var domainEntities = ChangeTracker.Entries<BaseEntity>()
             .Where(x => x.Entity.DomainEvents.Any())
             .ToList();

         var domainEvents = domainEntities
             .SelectMany(x => x.Entity.DomainEvents)
             .ToList();

         foreach (var domainEvent in domainEvents)
         {
             await _mediator.Publish(domainEvent);
         }

         domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents());
     }

     public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
     {
         await DispatchDomainEventsAsync();
         return await base.SaveChangesAsync(cancellationToken);
     }
     ```

4. **Create Event Handlers**:
   - Implement `INotificationHandler<T>` for each event.
   - Example:
     ```csharp
     public class UserCreatedEventHandler : INotificationHandler<UserCreatedEvent>
     {
         public Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
         {
             Console.WriteLine($"User created: {notification.User.FirstName}");
             // Send email, create profile, etc.
             return Task.CompletedTask;
         }
     }
     ```

5. **Register MediatR**:
   - In your `Startup.cs` or `Program.cs`:
     ```csharp
     services.AddMediatR(typeof(Program).Assembly);
     ```

---

### **Subscribing to Events**
Subscribers are typically implemented as `INotificationHandler<T>` classes. These handlers are automatically invoked by MediatR when an event is published.

#### **Example Subscriber**
For the `UserCreatedEvent`:
```csharp
public class WelcomeEmailHandler : INotificationHandler<UserCreatedEvent>
{
    private readonly IEmailService _emailService;

    public WelcomeEmailHandler(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        var user = notification.User;
        await _emailService.SendAsync(user.Email, "Welcome!", "Thank you for registering.");
    }
}
```

---

### **Summary**
- **Domain Events** decouple the domain logic from external concerns.
- **MediatR** is an excellent library for implementing domain events in a clean and scalable way.
- Events are raised in the domain model, published via a mediator, and handled by subscribers.
- This approach ensures a maintainable, extensible, and testable architecture.