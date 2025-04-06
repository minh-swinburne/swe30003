using SmartRide.Application.DTOs.Locations;

namespace SmartRide.Application.Queries.Locations;

public class GetLocationByIdQuery : BaseQuery<GetLocationResponseDTO>
{
    public Guid LocationId { get; set; }
}
