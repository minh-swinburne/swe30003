using FluentValidation;
using SmartRide.Application.Commands.Vehicles;
using SmartRide.Common.Constants;
using SmartRide.Common.Responses.Errors;

namespace SmartRide.Application.Commands.Vehicles.Validators;

public class UpdateVehicleCommandValidator : AbstractValidator<UpdateVehicleCommand>
{
    public UpdateVehicleCommandValidator()
    {
        RuleFor(x => x.VehicleId)
            .NotEmpty()
            .WithMessage(VehicleErrors.ID_EMPTY.Message)
            .WithErrorCode(VehicleErrors.ID_EMPTY.Code);

        RuleFor(x => x.Plate)
            .MaximumLength(VehicleConstants.PlateMaxLength)
            .WithMessage(VehicleErrors.PLATE_TOO_LONG.Message)
            .WithErrorCode(VehicleErrors.PLATE_TOO_LONG.Code)
            .Matches(VehicleConstants.PlatePattern)
            .WithMessage(VehicleErrors.PLATE_INVALID.Message)
            .WithErrorCode(VehicleErrors.PLATE_INVALID.Code)
            .When(x => !string.IsNullOrWhiteSpace(x.Plate));

        RuleFor(x => x.Make)
            .MaximumLength(VehicleConstants.MakeMaxLength)
            .WithMessage(VehicleErrors.MAKE_TOO_LONG.Message)
            .WithErrorCode(VehicleErrors.MAKE_TOO_LONG.Code)
            .When(x => !string.IsNullOrWhiteSpace(x.Make));

        RuleFor(x => x.Model)
            .MaximumLength(VehicleConstants.ModelMaxLength)
            .WithMessage(VehicleErrors.MODEL_TOO_LONG.Message)
            .WithErrorCode(VehicleErrors.MODEL_TOO_LONG.Code)
            .When(x => !string.IsNullOrWhiteSpace(x.Model));

        RuleFor(x => x.Year)
            .InclusiveBetween(1886, DateTime.UtcNow.Year)
            .WithMessage(VehicleErrors.YEAR_INVALID.Message)
            .WithErrorCode(VehicleErrors.YEAR_INVALID.Code)
            .When(x => x.Year.HasValue);

        RuleFor(x => x.RegisteredDate)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage(VehicleErrors.REGISTERED_DATE_INVALID.Message)
            .WithErrorCode(VehicleErrors.REGISTERED_DATE_INVALID.Code)
            .When(x => x.RegisteredDate.HasValue);
    }
}
