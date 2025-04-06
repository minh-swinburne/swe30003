using FluentValidation.TestHelper;
using SmartRide.Application.Commands.Vehicles;
using SmartRide.Application.Validators.Vehicles;
using SmartRide.Common.Constants;

namespace SmartRide.UnitTests.Application.Validators.Vehicles;

public class CreateVehicleCommandValidatorTests
{
    private readonly CreateVehicleCommandValidator _validator;

    public CreateVehicleCommandValidatorTests()
    {
        _validator = new CreateVehicleCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_UserId_Is_Empty()
    {
        var command = new CreateVehicleCommand { UserId = Guid.Empty };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public void Should_Have_Error_When_Vin_Is_Empty()
    {
        var command = new CreateVehicleCommand { Vin = string.Empty };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Vin);
    }

    [Fact]
    public void Should_Have_Error_When_Vin_Exceeds_Max_Length()
    {
        var command = new CreateVehicleCommand { Vin = new string('A', VehicleConstants.VinMaxLength + 1) };
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
    public void Should_Have_Error_When_Year_Is_Out_Of_Range()
    {
        var command = new CreateVehicleCommand { Year = 1800 };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Year);
    }

    [Fact]
    public void Should_Have_Error_When_RegisteredDate_Is_In_The_Future()
    {
        var command = new CreateVehicleCommand { RegisteredDate = DateTime.UtcNow.AddDays(1) };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.RegisteredDate);
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
