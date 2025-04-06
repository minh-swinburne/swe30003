using FluentValidation.TestHelper;
using SmartRide.Application.Commands.Locations;
using SmartRide.Application.Validators.Locations;
using SmartRide.Common.Constants;

namespace SmartRide.UnitTests.Application.Validators.Locations;

public class UpdateLocationCommandValidatorTests
{
  private readonly UpdateLocationCommandValidator _validator;

  public UpdateLocationCommandValidatorTests()
  {
    _validator = new UpdateLocationCommandValidator();
  }

  [Fact]
  public void Should_Have_Error_When_LocationId_Is_Empty()
  {
    var command = new UpdateLocationCommand { LocationId = Guid.Empty };
    var result = _validator.TestValidate(command);
    result.ShouldHaveValidationErrorFor(x => x.LocationId);
  }

  [Fact]
  public void Should_Have_Error_When_Address_Exceeds_Max_Length()
  {
    var command = new UpdateLocationCommand { Address = new string('A', LocationConstants.AddressMaxLength + 1) };
    var result = _validator.TestValidate(command);
    result.ShouldHaveValidationErrorFor(x => x.Address);
  }

  [Fact]
  public void Should_Have_Error_When_Latitude_Is_Out_Of_Range()
  {
    var command = new UpdateLocationCommand { Latitude = -100 };
    var result = _validator.TestValidate(command);
    result.ShouldHaveValidationErrorFor(x => x.Latitude);
  }

  [Fact]
  public void Should_Have_Error_When_Longitude_Is_Out_Of_Range()
  {
    var command = new UpdateLocationCommand { Longitude = 200 };
    var result = _validator.TestValidate(command);
    result.ShouldHaveValidationErrorFor(x => x.Longitude);
  }

  [Fact]
  public void Should_Not_Have_Error_When_Command_Is_Valid()
  {
    var command = new UpdateLocationCommand
    {
      LocationId = Guid.NewGuid(),
      Address = "456 Elm St",
      Latitude = 40.0,
      Longitude = -75.0,
      UserId = Guid.NewGuid()
    };
    var result = _validator.TestValidate(command);
    result.ShouldNotHaveAnyValidationErrors();
  }
}
