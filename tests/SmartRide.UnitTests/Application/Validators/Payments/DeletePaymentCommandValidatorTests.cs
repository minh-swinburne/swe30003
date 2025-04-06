using FluentValidation.TestHelper;
using SmartRide.Application.Commands.Payments;
using SmartRide.Application.Validators.Payments;

namespace SmartRide.UnitTests.Application.Validators.Payments;

public class DeletePaymentCommandValidatorTests
{
    private readonly DeletePaymentCommandValidator _validator;

    public DeletePaymentCommandValidatorTests()
    {
        _validator = new DeletePaymentCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_PaymentId_Is_Empty()
    {
        var command = new DeletePaymentCommand { PaymentId = Guid.Empty };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.PaymentId);
    }

    [Fact]
    public void Should_Not_Have_Error_When_PaymentId_Is_Valid()
    {
        var command = new DeletePaymentCommand { PaymentId = Guid.NewGuid() };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.PaymentId);
    }
}
