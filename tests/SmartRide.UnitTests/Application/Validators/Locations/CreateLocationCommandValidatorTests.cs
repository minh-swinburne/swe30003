using FluentValidation.TestHelper;
using SmartRide.Application.Commands.Locations;
using SmartRide.Application.Validators.Locations;
using SmartRide.Common.Constants;

namespace SmartRide.UnitTests.Application.Validators.Locations;

public class CreateLocationCommandValidatorTests
{
    private readonly CreateLocationCommandValidator _validator;

    public CreateLocationCommandValidatorTests()
    {
        _validator = new CreateLocationCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Address_Is_Empty()
    {
        var command = new CreateLocationCommand { Address = string.Empty };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Address);
    }

    [Fact]
    public void Should_Have_Error_When_Address_Exceeds_Max_Length()
    {
        var command = new CreateLocationCommand { Address = new string('A', LocationConstants.AddressMaxLength + 1) };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Address);
    }

    [Fact]
    public void Should_Have_Error_When_Latitude_Is_Out_Of_Range()
    {
        var command = new CreateLocationCommand { Latitude = 100 };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Latitude);
    }

    [Fact]
    public void Should_Have_Error_When_Longitude_Is_Out_Of_Range()
    {
        var command = new CreateLocationCommand { Longitude = 200 };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Longitude);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        var command = new CreateLocationCommand
        {
            Address = "123 Main St",
            Latitude = 45.0,
            Longitude = -93.0,
            UserId = Guid.NewGuid()
        };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
