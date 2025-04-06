using FluentValidation;
using SmartRide.Application.Commands.Rides;
using SmartRide.Common.Constants;
using SmartRide.Common.Responses.Errors;

namespace SmartRide.Application.Validators.Rides;

public class CreateRideCommandValidator : AbstractValidator<CreateRideCommand>
{
    public CreateRideCommandValidator()
    {
        RuleFor(x => x.PassengerId)
            .NotEmpty()
            .WithMessage(RideErrors.PASSENGER_ID_EMPTY.Message)
            .WithErrorCode(RideErrors.PASSENGER_ID_EMPTY.Code);

        RuleFor(x => x.PickupLocationId)
            .NotEmpty()
            .WithMessage(RideErrors.PICKUP_LOCATION_ID_EMPTY.Message)
            .WithErrorCode(RideErrors.PICKUP_LOCATION_ID_EMPTY.Code);

        RuleFor(x => x.DestinationId)
            .NotEmpty()
            .WithMessage(RideErrors.DESTINATION_ID_EMPTY.Message)
            .WithErrorCode(RideErrors.DESTINATION_ID_EMPTY.Code);

        RuleFor(x => x.Fare)
            .InclusiveBetween(RideConstants.MinFare, RideConstants.MaxFare)
            .WithMessage(RideErrors.FARE_INVALID.Message)
            .WithErrorCode(RideErrors.FARE_INVALID.Code);

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

        RuleFor(x => x.ArrivalETA)
            .GreaterThanOrEqualTo(DateTime.UtcNow)
            .WithMessage(RideErrors.ARRIVAL_ETA_IN_PAST.Message)
            .WithErrorCode(RideErrors.ARRIVAL_ETA_IN_PAST.Code)
            .When(x => x.ArrivalETA.HasValue);
    }
}
