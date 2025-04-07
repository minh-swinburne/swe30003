using FluentValidation;
using SmartRide.Application.Commands.Rides;
using SmartRide.Common.Constants;
using SmartRide.Common.Responses.Errors;

namespace SmartRide.Application.Validators.Rides;

public class UpdateRideCommandValidator : AbstractValidator<UpdateRideCommand>
{
    public UpdateRideCommandValidator()
    {
        RuleFor(x => x.RideId)
            .NotEmpty()
            .WithMessage(RideErrors.ID_EMPTY.Message)
            .WithErrorCode(RideErrors.ID_EMPTY.Code);

        RuleFor(x => x.Notes)
            .MaximumLength(RideConstants.NotesMaxLength)
            .WithMessage(RideErrors.NOTES_TOO_LONG.Message)
            .WithErrorCode(RideErrors.NOTES_TOO_LONG.Code)
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));

        RuleFor(x => x.PickupETA)
            .GreaterThanOrEqualTo(DateTime.UtcNow)
            .WithMessage(RideErrors.PICKUP_ETA_IN_PAST.Message)
            .WithErrorCode(RideErrors.PICKUP_ETA_IN_PAST.Code)
            .When(x => x.PickupETA.HasValue);

        RuleFor(x => x.PickupATA)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage(RideErrors.PICKUP_ATA_IN_FUTURE.Message)
            .WithErrorCode(RideErrors.PICKUP_ATA_IN_FUTURE.Code)
            .When(x => x.PickupATA.HasValue);

        RuleFor(x => x.ArrivalATA)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage(RideErrors.ARRIVAL_ATA_IN_FUTURE.Message)
            .WithErrorCode(RideErrors.ARRIVAL_ATA_IN_FUTURE.Code)
            .When(x => x.ArrivalATA.HasValue);

        RuleFor(x => x.ArrivalETA)
            .GreaterThanOrEqualTo(DateTime.UtcNow)
            .WithMessage(RideErrors.ARRIVAL_ETA_IN_PAST.Message)
            .WithErrorCode(RideErrors.ARRIVAL_ETA_IN_PAST.Code)
            .When(x => x.ArrivalETA.HasValue);
    }
}
