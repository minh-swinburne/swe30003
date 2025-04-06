using FluentValidation.TestHelper;
using SmartRide.Application.Commands.Vehicles;
using SmartRide.Application.Validators.Vehicles;
using SmartRide.Common.Constants;

namespace SmartRide.UnitTests.Application.Validators.Vehicles;

public class UpdateVehicleCommandValidatorTests
{
    private readonly UpdateVehicleCommandValidator _validator;

    public UpdateVehicleCommandValidatorTests()
    {
        _validator = new UpdateVehicleCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_VehicleId_Is_Empty()
    {
        var command = new UpdateVehicleCommand { VehicleId = Guid.Empty };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.VehicleId);
    }

    [Fact]
    public void Should_Have_Error_When_Plate_Is_Invalid()
    {
        var command = new UpdateVehicleCommand { VehicleId = Guid.NewGuid(), Plate = "INVALID_PLATE!" };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Plate);
    }

    [Fact]
    public void Should_Have_Error_When_Year_Is_Out_Of_Range()
    {
        var command = new UpdateVehicleCommand { VehicleId = Guid.NewGuid(), Year = 1800 };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Year);
    }

    [Fact]
    public void Should_Have_Error_When_RegisteredDate_Is_In_The_Future()
    {
        var command = new UpdateVehicleCommand { VehicleId = Guid.NewGuid(), RegisteredDate = DateTime.UtcNow.AddDays(1) };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.RegisteredDate);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        var command = new UpdateVehicleCommand
        {
            VehicleId = Guid.NewGuid(),
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
