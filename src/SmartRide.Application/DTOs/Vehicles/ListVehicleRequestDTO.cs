using SmartRide.Domain.Enums;

namespace SmartRide.Application.DTOs.Vehicles;

public class ListVehicleRequestDTO : BaseRequestDTO
{
    public Guid? DriverId { get; init; }
    public VehicleTypeEnum? VehicleType { get; init; }
    public string? Make { get; init; }
    public string? Model { get; init; }
    public int? Year { get; init; }
    public DateTime? RegisteredDateFrom { get; init; }
    public DateTime? RegisteredDateTo { get; init; }

    public string? OrderBy { get; init; }
    public bool Ascending { get; init; } = true;
    public int PageSize { get; init; } = 10;
    public int PageNo { get; init; } = 1;
}
