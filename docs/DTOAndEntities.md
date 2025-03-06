# Struct for DTO and Handling Different Types of Entities

## Question

- For DTOs can I use `struct` instead of `class`? Does it affect things like Handler and Validator?
- Here's my `Entity` working as a base class for other entities from the database, defined in the Domain layer:
```
namespace SmartRide.Domain.Entities;

public abstract class Entity
{
    public required string Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public Entity()
    {
        Id = Guid.NewGuid().ToString();
    }

    public virtual string GetId()
    {
        return Id;
    }

    public void UpdateTimestamp()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}
```
It currently assumes that every entity has a string field `Id` that will be auto-generated as a Guid. Why this is fine for key entities, there are some "enum" entities (like user role table) that don't use string field but auto-increment int id. is there any particular term for this kind of table? And how should I handle this, should I create separate classes to handle two different kinds of entities?

---

