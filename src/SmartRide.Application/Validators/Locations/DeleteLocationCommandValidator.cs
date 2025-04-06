using FluentValidation;
using SmartRide.Application.Commands.Locations;
using SmartRide.Common.Responses.Errors;

namespace SmartRide.Application.Validators.Locations;

public class DeleteLocationCommandValidator : AbstractValidator<DeleteLocationCommand>
{
    public DeleteLocationCommandValidator()
    {
        RuleFor(x => x.LocationId)
            .NotEmpty()
            .WithMessage(LocationErrors.ID_EMPTY.Message)
            .WithErrorCode(LocationErrors.ID_EMPTY.Code);
    }
}
