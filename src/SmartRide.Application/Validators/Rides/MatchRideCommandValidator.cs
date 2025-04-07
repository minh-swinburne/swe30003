using FluentValidation;
using SmartRide.Application.Commands.Rides;
using SmartRide.Common.Responses.Errors;

namespace SmartRide.Application.Validators.Rides;

public class MatchRideCommandValidator : AbstractValidator<MatchRideCommand>
{
    public MatchRideCommandValidator()
    {
        RuleFor(x => x.RideId)
            .NotEmpty()
            .WithMessage(RideErrors.ID_EMPTY.Message)
            .WithErrorCode(RideErrors.ID_EMPTY.Code);

        RuleFor(x => x.DriverId)
            .NotEmpty()
            .WithMessage(RideErrors.DRIVER_ID_EMPTY.Message)
            .WithErrorCode(RideErrors.DRIVER_ID_EMPTY.Code);

        RuleFor(x => x.VehicleId)
            .NotEmpty()
            .WithMessage(RideErrors.VEHICLE_ID_EMPTY.Message)
            .WithErrorCode(RideErrors.VEHICLE_ID_EMPTY.Code);
    }
}