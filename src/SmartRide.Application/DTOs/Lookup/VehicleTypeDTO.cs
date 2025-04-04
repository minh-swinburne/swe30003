namespace SmartRide.Application.DTOs.Lookup;

public class VehicleTypeDTO
{
    public byte VehicleTypeId { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public int Capacity { get; init; }
}
