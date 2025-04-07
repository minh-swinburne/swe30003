using FluentValidation.TestHelper;
using SmartRide.Application.Commands.Rides;
using SmartRide.Application.Validators.Rides;

namespace SmartRide.UnitTests.Application.Validators.Rides;

public class MatchRideCommandValidatorTests
{
    private readonly MatchRideCommandValidator _validator;

    public MatchRideCommandValidatorTests()
    {
        _validator = new MatchRideCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_RideId_Is_Empty()
    {
        var command = new MatchRideCommand { RideId = Guid.Empty };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.RideId);
    }

    [Fact]
    public void Should_Have_Error_When_DriverId_Is_Empty()
    {
        var command = new MatchRideCommand { DriverId = Guid.Empty };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.DriverId);
    }

    [Fact]
    public void Should_Have_Error_When_VehicleId_Is_Empty()
    {
        var command = new MatchRideCommand { VehicleId = Guid.Empty };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.VehicleId);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        var command = new MatchRideCommand
        {
            RideId = Guid.NewGuid(),
            DriverId = Guid.NewGuid(),
            VehicleId = Guid.NewGuid()
        };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
