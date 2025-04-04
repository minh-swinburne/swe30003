namespace SmartRide.Application.DTOs.Lookup;

public class RoleDTO
{
    public byte RoleId { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
}
