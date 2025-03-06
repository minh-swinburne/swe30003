namespace SmartRide.Domain.Entities.Base;

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
