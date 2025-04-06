using SmartRide.Application.DTOs.Vehicles;
using SmartRide.Common.Interfaces;
using SmartRide.Domain.Enums;

namespace SmartRide.Application.Queries.Vehicles;

public class ListVehicleQuery : BaseQuery<List<ListVehicleResponseDTO>>, IPageable, ISortable
{
    public Guid? DriverId { get; init; }
    public VehicleTypeEnum? VehicleType { get; init; }
    public string? Make { get; init; }
    public string? Model { get; init; }
    public int? Year { get; init; }
    public DateTime? RegisteredDateFrom { get; init; }
    public DateTime? RegisteredDateTo { get; init; }

    public string? OrderBy { get; set; }
    public bool Ascending { get; set; } = true;
    public int PageSize { get; set; } = 10;
    public int PageNo { get; set; } = 1;
}
