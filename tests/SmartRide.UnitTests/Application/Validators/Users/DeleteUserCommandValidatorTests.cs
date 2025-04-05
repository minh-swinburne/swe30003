using FluentValidation.TestHelper;
using SmartRide.Application.Commands.Users;
using SmartRide.Application.Commands.Users.Validators;

namespace SmartRide.UnitTests.Application.Validators.Users;

public class DeleteUserCommandValidatorTests
{
  private readonly DeleteUserCommandValidator _validator;

  public DeleteUserCommandValidatorTests()
  {
    _validator = new DeleteUserCommandValidator();
  }

  [Fact]
  public void Should_Have_Error_When_UserId_Is_Empty()
  {
    var command = new DeleteUserCommand { UserId = Guid.Empty };
    var result = _validator.TestValidate(command);
    result.ShouldHaveValidationErrorFor(x => x.UserId);
  }

  [Fact]
  public void Should_Not_Have_Error_When_UserId_Is_Valid()
  {
    var command = new DeleteUserCommand { UserId = Guid.NewGuid() };
    var result = _validator.TestValidate(command);
    result.ShouldNotHaveValidationErrorFor(x => x.UserId);
  }
}
