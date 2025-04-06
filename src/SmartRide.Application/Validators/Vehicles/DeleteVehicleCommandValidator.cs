using FluentValidation;
using SmartRide.Application.Commands.Vehicles;
using SmartRide.Common.Responses.Errors;

namespace SmartRide.Application.Validators.Vehicles;

public class DeleteVehicleCommandValidator : AbstractValidator<DeleteVehicleCommand>
{
    public DeleteVehicleCommandValidator()
    {
        RuleFor(x => x.VehicleId)
            .NotEmpty()
            .WithMessage(VehicleErrors.ID_EMPTY.Message)
            .WithErrorCode(VehicleErrors.ID_EMPTY.Code);
    }
}
