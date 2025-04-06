using FluentValidation.TestHelper;
using SmartRide.Application.Commands.Locations;
using SmartRide.Application.Validators.Locations;

namespace SmartRide.UnitTests.Application.Validators.Locations;

public class DeleteLocationCommandValidatorTests
{
    private readonly DeleteLocationCommandValidator _validator;

    public DeleteLocationCommandValidatorTests()
    {
        _validator = new DeleteLocationCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_LocationId_Is_Empty()
    {
        var command = new DeleteLocationCommand { LocationId = Guid.Empty };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.LocationId);
    }

    [Fact]
    public void Should_Not_Have_Error_When_LocationId_Is_Valid()
    {
        var command = new DeleteLocationCommand { LocationId = Guid.NewGuid() };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.LocationId);
    }
}
