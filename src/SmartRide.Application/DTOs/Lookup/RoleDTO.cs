using SmartRide.Domain.Enums;

namespace SmartRide.Application.DTOs.Lookup;

public class RoleDTO
{
    public RoleEnum RoleId { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
}
