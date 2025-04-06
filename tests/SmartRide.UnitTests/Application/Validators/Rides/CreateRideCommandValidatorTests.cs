using FluentValidation.TestHelper;
using SmartRide.Application.Commands.Rides;
using SmartRide.Application.Validators.Rides;
using SmartRide.Common.Constants;

namespace SmartRide.UnitTests.Application.Validators.Rides;

public class CreateRideCommandValidatorTests
{
    private readonly CreateRideCommandValidator _validator;

    public CreateRideCommandValidatorTests()
    {
        _validator = new CreateRideCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_PassengerId_Is_Empty()
    {
        var command = new CreateRideCommand { PassengerId = Guid.Empty };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.PassengerId);
    }

    [Fact]
    public void Should_Have_Error_When_PickupLocationId_Is_Empty()
    {
        var command = new CreateRideCommand { PickupLocationId = Guid.Empty };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.PickupLocationId);
    }

    [Fact]
    public void Should_Have_Error_When_DestinationId_Is_Empty()
    {
        var command = new CreateRideCommand { DestinationId = Guid.Empty };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.DestinationId);
    }

    [Fact]
    public void Should_Have_Error_When_Fare_Is_Out_Of_Range()
    {
        var command = new CreateRideCommand { Fare = RideConstants.MaxFare + 1 };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Fare);
    }

    [Fact]
    public void Should_Have_Error_When_Notes_Exceed_Max_Length()
    {
        var command = new CreateRideCommand { Notes = new string('A', RideConstants.NotesMaxLength + 1) };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Notes);
    }

    [Fact]
    public void Should_Have_Error_When_PickupETA_Is_In_The_Past()
    {
        var command = new CreateRideCommand { PickupETA = DateTime.UtcNow.AddMinutes(-1) };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.PickupETA);
    }

    [Fact]
    public void Should_Have_Error_When_ArrivalETA_Is_In_The_Past()
    {
        var command = new CreateRideCommand { ArrivalETA = DateTime.UtcNow.AddMinutes(-1) };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.ArrivalETA);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        var command = new CreateRideCommand
        {
            PassengerId = Guid.NewGuid(),
            PickupLocationId = Guid.NewGuid(),
            DestinationId = Guid.NewGuid(),
            Fare = 100,
            Notes = "Valid notes",
            PickupETA = DateTime.UtcNow.AddMinutes(10),
            ArrivalETA = DateTime.UtcNow.AddMinutes(20)
        };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
