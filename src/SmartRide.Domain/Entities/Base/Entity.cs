namespace SmartRide.Domain.Entities.Base;

public abstract class Entity
{
    public required string Id { get; set; }
    public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedTime { get; set; } = DateTime.UtcNow;

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
        UpdatedTime = DateTime.UtcNow;
    }
}
