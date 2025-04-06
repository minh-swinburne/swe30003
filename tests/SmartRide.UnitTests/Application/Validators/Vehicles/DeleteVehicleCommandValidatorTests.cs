using FluentValidation.TestHelper;
using SmartRide.Application.Commands.Vehicles;
using SmartRide.Application.Validators.Vehicles;

namespace SmartRide.UnitTests.Application.Validators.Vehicles;

public class DeleteVehicleCommandValidatorTests
{
    private readonly DeleteVehicleCommandValidator _validator;

    public DeleteVehicleCommandValidatorTests()
    {
        _validator = new DeleteVehicleCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_VehicleId_Is_Empty()
    {
        var command = new DeleteVehicleCommand { VehicleId = Guid.Empty };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.VehicleId);
    }

    [Fact]
    public void Should_Not_Have_Error_When_VehicleId_Is_Valid()
    {
        var command = new DeleteVehicleCommand { VehicleId = Guid.NewGuid() };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.VehicleId);
    }
}
