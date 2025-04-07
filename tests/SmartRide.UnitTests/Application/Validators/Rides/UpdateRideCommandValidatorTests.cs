using FluentValidation.TestHelper;
using SmartRide.Application.Commands.Rides;
using SmartRide.Application.Validators.Rides;
using SmartRide.Common.Constants;

namespace SmartRide.UnitTests.Application.Validators.Rides;

public class UpdateRideCommandValidatorTests
{
    private readonly UpdateRideCommandValidator _validator;

    public UpdateRideCommandValidatorTests()
    {
        _validator = new UpdateRideCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_RideId_Is_Empty()
    {
        var command = new UpdateRideCommand { RideId = Guid.Empty };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.RideId);
    }

    [Fact]
    public void Should_Have_Error_When_Notes_Exceed_Max_Length()
    {
        var command = new UpdateRideCommand { Notes = new string('A', RideConstants.NotesMaxLength + 1) };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Notes);
    }

    [Fact]
    public void Should_Have_Error_When_PickupETA_Is_In_The_Past()
    {
        var command = new UpdateRideCommand { PickupETA = DateTime.UtcNow.AddMinutes(-1) };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.PickupETA);
    }

    [Fact]
    public void Should_Have_Error_When_ArrivalETA_Is_In_The_Past()
    {
        var command = new UpdateRideCommand { ArrivalETA = DateTime.UtcNow.AddMinutes(-1) };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.ArrivalETA);
    }

    [Fact]
    public void Should_Have_Error_When_PickupATA_Is_In_The_Future()
    {
        var command = new UpdateRideCommand { PickupATA = DateTime.UtcNow.AddMinutes(1) };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.PickupATA);
    }

    [Fact]
    public void Should_Have_Error_When_ArrivalATA_Is_In_The_Future()
    {
        var command = new UpdateRideCommand { ArrivalATA = DateTime.UtcNow.AddMinutes(1) };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.ArrivalATA);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        var command = new UpdateRideCommand
        {
            RideId = Guid.NewGuid(),
            Notes = "Valid notes",
            PickupETA = DateTime.UtcNow.AddMinutes(10),
            ArrivalETA = DateTime.UtcNow.AddMinutes(20),
            PickupATA = DateTime.UtcNow.AddMinutes(-10),
            ArrivalATA = DateTime.UtcNow.AddMinutes(-5)
        };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
