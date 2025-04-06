using FluentValidation.TestHelper;
using SmartRide.Application.Commands.Payments;
using SmartRide.Application.Validators.Payments;
using SmartRide.Common.Constants;
using SmartRide.Domain.Enums;

namespace SmartRide.UnitTests.Application.Validators.Payments;

public class CreatePaymentCommandValidatorTests
{
    private readonly CreatePaymentCommandValidator _validator;

    public CreatePaymentCommandValidatorTests()
    {
        _validator = new CreatePaymentCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_RideId_Is_Empty()
    {
        var command = new CreatePaymentCommand { RideId = Guid.Empty, Amount = 100, PaymentMethodId = 0 };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.RideId);
    }

    [Fact]
    public void Should_Have_Error_When_Amount_Is_Out_Of_Range()
    {
        var command = new CreatePaymentCommand { RideId = Guid.NewGuid(), Amount = PaymentConstants.MaxAmount + 1, PaymentMethodId = 0 };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Amount);
    }

    [Fact]
    public void Should_Have_Error_When_PaymentMethodId_Is_Invalid()
    {
        var command = new CreatePaymentCommand { RideId = Guid.NewGuid(), Amount = 100, PaymentMethodId = (PaymentMethodEnum)999 };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.PaymentMethodId);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        var command = new CreatePaymentCommand { RideId = Guid.NewGuid(), Amount = 100, PaymentMethodId = PaymentMethodEnum.Cash };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
