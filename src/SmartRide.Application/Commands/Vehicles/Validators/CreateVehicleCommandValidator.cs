using FluentValidation;
using SmartRide.Application.Commands.Vehicles;
using SmartRide.Common.Constants;
using SmartRide.Common.Responses.Errors;

namespace SmartRide.Application.Commands.Vehicles.Validators;

public class CreateVehicleCommandValidator : AbstractValidator<CreateVehicleCommand>
{
    public CreateVehicleCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage(VehicleErrors.ID_EMPTY.Message)
            .WithErrorCode(VehicleErrors.ID_EMPTY.Code);

        RuleFor(x => x.Vin)
            .NotEmpty()
            .WithMessage(VehicleErrors.VIN_EMPTY.Message)
            .WithErrorCode(VehicleErrors.VIN_EMPTY.Code)
            .MaximumLength(VehicleConstants.VinMaxLength)
            .WithMessage(VehicleErrors.VIN_TOO_LONG.Message)
            .WithErrorCode(VehicleErrors.VIN_TOO_LONG.Code)
            .Matches(VehicleConstants.VinPattern)
            .WithMessage(VehicleErrors.VIN_INVALID.Message)
            .WithErrorCode(VehicleErrors.VIN_INVALID.Code);

        RuleFor(x => x.Plate)
            .NotEmpty()
            .WithMessage(VehicleErrors.PLATE_EMPTY.Message)
            .WithErrorCode(VehicleErrors.PLATE_EMPTY.Code)
            .MaximumLength(VehicleConstants.PlateMaxLength)
            .WithMessage(VehicleErrors.PLATE_TOO_LONG.Message)
            .WithErrorCode(VehicleErrors.PLATE_TOO_LONG.Code)
            .Matches(VehicleConstants.PlatePattern)
            .WithMessage(VehicleErrors.PLATE_INVALID.Message)
            .WithErrorCode(VehicleErrors.PLATE_INVALID.Code);

        RuleFor(x => x.Make)
            .NotEmpty()
            .WithMessage(VehicleErrors.MAKE_EMPTY.Message)
            .WithErrorCode(VehicleErrors.MAKE_EMPTY.Code)
            .MaximumLength(VehicleConstants.MakeMaxLength)
            .WithMessage(VehicleErrors.MAKE_TOO_LONG.Message)
            .WithErrorCode(VehicleErrors.MAKE_TOO_LONG.Code);

        RuleFor(x => x.Model)
            .NotEmpty()
            .WithMessage(VehicleErrors.MODEL_EMPTY.Message)
            .WithErrorCode(VehicleErrors.MODEL_EMPTY.Code)
            .MaximumLength(VehicleConstants.ModelMaxLength)
            .WithMessage(VehicleErrors.MODEL_TOO_LONG.Message)
            .WithErrorCode(VehicleErrors.MODEL_TOO_LONG.Code);

        RuleFor(x => x.Year)
            .InclusiveBetween(1886, DateTime.UtcNow.Year)
            .WithMessage(VehicleErrors.YEAR_INVALID.Message)
            .WithErrorCode(VehicleErrors.YEAR_INVALID.Code);

        RuleFor(x => x.RegisteredDate)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage(VehicleErrors.REGISTERED_DATE_INVALID.Message)
            .WithErrorCode(VehicleErrors.REGISTERED_DATE_INVALID.Code);
    }
}
