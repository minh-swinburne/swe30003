namespace SmartRide.Domain.Entities.Base;

public abstract class LookupEntity
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
}
