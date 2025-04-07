using SmartRide.Application.DTOs.Rides;
using SmartRide.Common.Interfaces;
using SmartRide.Domain.Enums;

namespace SmartRide.Application.Queries.Rides;

public class ListRideQuery : BaseQuery<List<ListRideResponseDTO>>, IPageable, ISortable
{
    public Guid? PassengerId { get; set; }
    public Guid? DriverId { get; set; }
    public Guid? VehicleId { get; set; }
    public VehicleTypeEnum? VehicleType { get; set; }
    public RideTypeEnum? RideType { get; set; }
    public RideStatusEnum? RideStatus { get; set; }
    public DateTime? PickupDateFrom { get; set; }
    public DateTime? PickupDateTo { get; set; }

    public string? OrderBy { get; set; }
    public bool Ascending { get; set; } = true;
    public int PageSize { get; set; } = 10;
    public int PageNo { get; set; } = 1;
}
