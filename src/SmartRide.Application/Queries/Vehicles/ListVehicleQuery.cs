using SmartRide.Application.DTOs.Vehicles;
using SmartRide.Domain.Enums;

namespace SmartRide.Application.Queries.Vehicles;

public class ListVehicleQuery : BaseQuery<List<ListVehicleResponseDTO>>
{
    public string? Make { get; init; }
    public string? Model { get; init; }
    public int? Year { get; init; }
    public string? Vin { get; init; }
    public string? Plate { get; init; }
    public DateTime? RegisteredDateFrom { get; init; }
    public DateTime? RegisteredDateTo { get; init; }

    public string? OrderBy { get; init; }
    public bool Ascending { get; init; } = true;
    public int PageSize { get; init; } = 10;
    public int PageNo { get; init; } = 1;
    
    public List<VehicleTypeEnum>? VehicleTypes { get; init; }
    public bool MatchAllVehicleTypes { get; init; } = false;
}
