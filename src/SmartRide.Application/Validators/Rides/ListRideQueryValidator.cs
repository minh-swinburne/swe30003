using SmartRide.Application.Queries.Rides;
using SmartRide.Domain.Entities.Base;

namespace SmartRide.Application.Validators.Rides;

public class ListRideQueryValidator : BaseListQueryValidator<ListRideQuery, Ride>
{
    public ListRideQueryValidator()
    {
        // Additional validation rules for ListRideQuery can go here
    }
}
