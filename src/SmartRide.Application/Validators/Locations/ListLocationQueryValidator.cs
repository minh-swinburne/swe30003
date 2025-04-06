using SmartRide.Application.Queries.Locations;
using SmartRide.Domain.Entities.Base;

namespace SmartRide.Application.Validators.Locations;

public class ListLocationQueryValidator : BaseListQueryValidator<ListLocationQuery, Location>
{
    public ListLocationQueryValidator()
    {
        // Additional validation rules for ListLocationQuery can go here
    }
}
