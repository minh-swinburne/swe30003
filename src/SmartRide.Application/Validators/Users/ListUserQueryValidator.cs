using FluentValidation;
using SmartRide.Application.Queries.Users;
using SmartRide.Domain.Entities.Base;

namespace SmartRide.Application.Validators.Users;

public class ListUserQueryValidator : BaseListQueryValidator<ListUserQuery, User>
{
    public ListUserQueryValidator()
    {
        // Additional validation rules for ListUserQuery can go here
    }
}
