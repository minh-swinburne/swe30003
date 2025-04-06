using SmartRide.Application.DTOs.Locations;
using SmartRide.Common.Interfaces;

namespace SmartRide.Application.Queries.Locations;

public class ListLocationQuery : BaseQuery<List<ListLocationResponseDTO>>, ISortable, IPageable
{
    public Guid? UserId { get; set; }
    public string? Address { get; set; }
    public double? LatitudeFrom { get; set; }
    public double? LatitudeTo { get; set; }
    public double? LongitudeFrom { get; set; }
    public double? LongitudeTo { get; set; }

    public string? OrderBy { get; set; }
    public bool Ascending { get; set; } = true;
    public int PageSize { get; set; } = 10;
    public int PageNo { get; set; } = 1;
}
