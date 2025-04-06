using SmartRide.Domain.Enums;

namespace SmartRide.Application.DTOs.Vehicles;

public class CreateVehicleRequestDTO : BaseRequestDTO
{
    public required Guid UserId { get; init; }
    public required VehicleTypeEnum VehicleTypeId { get; init; }
    public required string Vin { get; init; }
    public required string Plate { get; init; }
    public required string Make { get; init; }
    public required string Model { get; init; }
    public required int Year { get; init; }
    public required DateTime RegisteredDate { get; init; }
}
