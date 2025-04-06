using FluentValidation.TestHelper;
using SmartRide.Application.Commands.Rides;
using SmartRide.Application.Validators.Rides;

namespace SmartRide.UnitTests.Application.Validators.Rides;

public class DeleteRideCommandValidatorTests
{
    private readonly DeleteRideCommandValidator _validator;

    public DeleteRideCommandValidatorTests()
    {
        _validator = new DeleteRideCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_RideId_Is_Empty()
    {
        var command = new DeleteRideCommand { RideId = Guid.Empty };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.RideId);
    }

    [Fact]
    public void Should_Not_Have_Error_When_RideId_Is_Valid()
    {
        var command = new DeleteRideCommand { RideId = Guid.NewGuid() };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.RideId);
    }
}
