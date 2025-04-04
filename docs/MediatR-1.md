# Question

is my registration for mediatR correct? btw where should the event handlers be implemented (probably infrastructure layer as well)? currently the events definitions are in Domain layer, should I move them to infrastructure layer (but if so how would the entity knows about events without breaking clean architecture)? how to subscribe a handler to an event? what interfaces do they need to implement to be recognized by mediatR (like IRequest and IRequestHandler in Application layer, but for events)?

---

# Answer

### **1. Is your registration for MediatR correct?**
Yes, your MediatR registration in the `DependencyInjection` class is correct. You are scanning the assembly for all event handlers using:

```csharp
services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
```

This ensures that all classes implementing MediatR interfaces (e.g., `INotificationHandler<T>`) in the `SmartRide.Infrastructure` assembly are registered automatically.

---

### **2. Where should event handlers be implemented?**
Event handlers should typically be implemented in the **Application layer**. Here's why:
- **Separation of Concerns**: The Application layer is responsible for orchestrating business logic and interacting with external systems (e.g., sending emails, logging, etc.).
- **Clean Architecture**: Keeping event handlers in the Application layer ensures that the Domain layer remains free of dependencies on infrastructure or external libraries like MediatR.

#### Example Folder Structure for Event Handlers:
```
SmartRide.Application/
    Events/
        Handlers/
            UserCreatedEventHandler.cs
            VehicleCreatedEventHandler.cs
```

---

### **3. Should event definitions be moved to the Infrastructure layer?**
No, event definitions should remain in the **Domain layer**. Here's why:
- **Domain Events Represent Business Logic**: Domain events are part of the domain model and represent significant business actions (e.g., `UserCreatedEvent`, `VehicleCreatedEvent`).
- **Clean Architecture**: Moving events to the Infrastructure layer would create a dependency from the Domain layer to the Infrastructure layer, violating the principles of clean architecture.

The Domain layer should define the events, and the Application layer should handle them.

---

### **4. How to subscribe a handler to an event?**
To subscribe a handler to an event in MediatR:
1. **Define the Event**:
   - Events should implement the `INotification` interface from MediatR.
   ```csharp
   public record UserCreatedEvent(User User) : INotification;
   ```

2. **Create the Event Handler**:
   - Handlers should implement the `INotificationHandler<T>` interface, where `T` is the event type.
   ```csharp
   public class UserCreatedEventHandler : INotificationHandler<UserCreatedEvent>
   {
       private readonly IEmailService _emailService;

       public UserCreatedEventHandler(IEmailService emailService)
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

3. **Register the Handler**:
   - MediatR automatically registers handlers when you scan the assembly using `AddMediatR`. Ensure the handler is in the same assembly or explicitly include its assembly in the registration.

---

### **5. What interfaces do event handlers need to implement?**
For events, MediatR uses the following interfaces:
- **Event Definition**: Implement `INotification` for events.
  ```csharp
  public record UserCreatedEvent(User User) : INotification;
  ```

- **Event Handler**: Implement `INotificationHandler<T>` for event handlers.
  ```csharp
  public class UserCreatedEventHandler : INotificationHandler<UserCreatedEvent>
  {
      public Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
      {
          // Handle the event
          return Task.CompletedTask;
      }
  }
  ```

---

### **6. Example Workflow**
Here’s how the workflow would look in your project:

1. **Domain Layer**:
   - Define the event in the Domain layer:
     ```csharp
     public record UserCreatedEvent(User User) : INotification;
     ```

   - Raise the event in the entity:
     ```csharp
     public override void OnSave(EntityState state)
     {
         base.OnSave(state);

         if (state == EntityState.Added)
         {
             AddDomainEvent(new UserCreatedEvent(this));
         }
     }
     ```

2. **Infrastructure Layer**:
   - Publish the event in `SmartRideDbContext`:
     ```csharp
     private void ProcessEntities()
     {
         foreach (var entry in ChangeTracker.Entries<BaseEntity>())
         {
             entry.Entity.OnSave(entry.State);

             foreach (var domainEvent in entry.Entity.DomainEvents)
             {
                 _mediator.Publish(domainEvent, cancellationToken: CancellationToken.None);
             }

             entry.Entity.ClearDomainEvents();
         }
     }
     ```

3. **Application Layer**:
   - Create the event handler:
     ```csharp
     public class UserCreatedEventHandler : INotificationHandler<UserCreatedEvent>
     {
         private readonly IEmailService _emailService;

         public UserCreatedEventHandler(IEmailService emailService)
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

### **7. Summary**
- **Event Definitions**: Stay in the Domain layer (e.g., `UserCreatedEvent`).
- **Event Handlers**: Belong in the Application layer (e.g., `UserCreatedEventHandler`).
- **MediatR Registration**: Your current registration is correct.
- **Interfaces**:
  - Events: Implement `INotification`.
  - Handlers: Implement `INotificationHandler<T>`.
- **Workflow**:
  - Raise events in the Domain layer.
  - Publish events in the Infrastructure layer (`SmartRideDbContext`).
  - Handle events in the Application layer.