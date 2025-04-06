using FluentValidation.TestHelper;
using SmartRide.Application.Commands.Vehicles;
using SmartRide.Application.Validators.Vehicles;

namespace SmartRide.UnitTests.Application.Validators.Vehicles;

public class CreateVehicleCommandValidatorTests
{
    private readonly CreateVehicleCommandValidator _validator;

    public CreateVehicleCommandValidatorTests()
    {
        _validator = new CreateVehicleCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Vin_Is_Empty()
    {
        var command = new CreateVehicleCommand { Vin = string.Empty };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Vin);
    }

    [Fact]
    public void Should_Have_Error_When_Plate_Is_Invalid()
    {
        var command = new CreateVehicleCommand { Plate = "INVALID_PLATE!" };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Plate);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        var command = new CreateVehicleCommand
        {
            UserId = Guid.NewGuid(),
            VehicleTypeId = 1,
            Vin = "1HGCM82633A123456",
            Plate = "ABC123",
            Make = "Toyota",
            Model = "Corolla",
            Year = 2020,
            RegisteredDate = DateTime.UtcNow.AddDays(-30)
        };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
