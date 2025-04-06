using FluentValidation;
using SmartRide.Common.Interfaces;

namespace SmartRide.Application.Validators;

public class ListQueryValidator : AbstractValidator<IPageable>
{
    public ListQueryValidator()
    {
        RuleFor(x => x.PageSize).GreaterThan(0);
        RuleFor(x => x.PageNo).GreaterThan(0);
    }
}
