using FluentValidation;
using SmartRide.Application.Commands.Rides;
using SmartRide.Common.Responses.Errors;

namespace SmartRide.Application.Validators.Rides;

public class DeleteRideCommandValidator : AbstractValidator<DeleteRideCommand>
{
    public DeleteRideCommandValidator()
    {
        RuleFor(x => x.RideId)
            .NotEmpty()
            .WithMessage(RideErrors.ID_EMPTY.Message)
            .WithErrorCode(RideErrors.ID_EMPTY.Code);
    }
}
