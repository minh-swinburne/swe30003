using SmartRide.Domain.Enums;
using SmartRide.Common.Interfaces;

namespace SmartRide.Application.DTOs.Rides;

public class ListRideRequestDTO : BaseRequestDTO, ISortable, IPageable
{
    public Guid? PassengerId { get; set; }
    public Guid? DriverId { get; set; }
    public Guid? VehicleId { get; set; }
    public RideTypeEnum? RideType { get; set; }
    public RideStatusEnum? RideStatus { get; set; }
    public DateTime? PickupATAFrom { get; set; }
    public DateTime? PickupATATo { get; set; }
    public DateTime? ArrivalATAFrom { get; set; }
    public DateTime? ArrivalATATo { get; set; }

    public string? OrderBy { get; set; }
    public bool Ascending { get; set; } = true;
    public int PageSize { get; set; } = 10;
    public int PageNo { get; set; } = 1;
}
