using FluentValidation.TestHelper;
using SmartRide.Application.Commands.Users;
using SmartRide.Application.Validators.Users;

namespace SmartRide.UnitTests.Application.Validators.Users;

public class UpdateUserCommandValidatorTests
{
    private readonly UpdateUserCommandValidator _validator;

    public UpdateUserCommandValidatorTests()
    {
        _validator = new UpdateUserCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_UserId_Is_Empty()
    {
        var command = new UpdateUserCommand { UserId = Guid.Empty };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public void Should_Not_Have_Error_When_UserId_Is_Valid()
    {
        var command = new UpdateUserCommand { UserId = Guid.NewGuid() };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public void Should_Have_Error_When_FirstName_Exceeds_Max_Length()
    {
        var command = new UpdateUserCommand { FirstName = new string('A', 51) };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }

    [Fact]
    public void Should_Not_Have_Error_When_FirstName_Is_Valid()
    {
        var command = new UpdateUserCommand { FirstName = "John" };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.FirstName);
    }

    [Fact]
    public void Should_Have_Error_When_LastName_Exceeds_Max_Length()
    {
        var command = new UpdateUserCommand { LastName = new string('B', 51) };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.LastName);
    }

    [Fact]
    public void Should_Not_Have_Error_When_LastName_Is_Valid()
    {
        var command = new UpdateUserCommand { LastName = "Doe" };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.LastName);
    }

    [Fact]
    public void Should_Have_Error_When_Email_Exceeds_Max_Length()
    {
        var command = new UpdateUserCommand { Email = new string('C', 256) + "@example.com" };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Invalid()
    {
        var command = new UpdateUserCommand { Email = "invalid-email" };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Email_Is_Valid()
    {
        var command = new UpdateUserCommand { Email = "johndoe@example.com" };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Have_Error_When_Phone_Exceeds_Max_Length()
    {
        var command = new UpdateUserCommand { Phone = new string('1', 46) };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Phone);
    }

    [Fact]
    public void Should_Have_Error_When_Phone_Is_Invalid()
    {
        var command = new UpdateUserCommand { Phone = "12345" };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Phone);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Phone_Is_Valid()
    {
        var command = new UpdateUserCommand { Phone = "+1234567890" };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Phone);
    }

    [Fact]
    public void Should_Have_Error_When_Password_Is_Too_Short()
    {
        var command = new UpdateUserCommand { Password = "Short1!" };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Have_Error_When_Password_Is_Too_Long()
    {
        var command = new UpdateUserCommand { Password = new string('A', 151) };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Have_Error_When_Password_Is_Weak()
    {
        var command = new UpdateUserCommand { Password = "weakpassword" };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Password_Is_Strong()
    {
        var command = new UpdateUserCommand { Password = "Secure@123" };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
    }
}
