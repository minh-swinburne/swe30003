using SmartRide.Domain.Enums;

namespace SmartRide.Application.DTOs.Vehicles;

public class GetVehicleByIdResponseDTO : BaseDTO
{
    public Guid VehicleId { get; init; }
    public Guid UserId { get; init; }
    public VehicleTypeEnum VehicleType { get; init; }
    public required string Vin { get; init; }
    public required string Plate { get; init; }
    public required string Make { get; init; }
    public required string Model { get; init; }
    public int Year { get; init; }
    public DateTime RegisteredDate { get; init; }
}
