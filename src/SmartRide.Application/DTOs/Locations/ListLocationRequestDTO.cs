using SmartRide.Common.Interfaces;

namespace SmartRide.Application.DTOs.Locations;

public class ListLocationRequestDTO : BaseRequestDTO, ISortable, IPageable
{
    public Guid? UserId { get; init; }
    public string? Address { get; init; }
    public double? LatitudeFrom { get; init; }
    public double? LatitudeTo { get; init; }
    public double? LongitudeFrom { get; init; }
    public double? LongitudeTo { get; init; }

    public string? OrderBy { get; set; }
    public bool Ascending { get; set; } = true;
    public int PageSize { get; set; } = 10;
    public int PageNo { get; set; } = 1;
}
