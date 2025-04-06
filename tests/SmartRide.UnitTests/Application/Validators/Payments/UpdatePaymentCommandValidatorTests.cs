using FluentValidation.TestHelper;
using SmartRide.Application.Commands.Payments;
using SmartRide.Application.Validators.Payments;
using SmartRide.Common.Constants;
using SmartRide.Domain.Enums;

namespace SmartRide.UnitTests.Application.Validators.Payments;

public class UpdatePaymentCommandValidatorTests
{
    private readonly UpdatePaymentCommandValidator _validator;

    public UpdatePaymentCommandValidatorTests()
    {
        _validator = new UpdatePaymentCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_PaymentId_Is_Empty()
    {
        var command = new UpdatePaymentCommand { PaymentId = Guid.Empty };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.PaymentId);
    }

    [Fact]
    public void Should_Have_Error_When_Amount_Is_Out_Of_Range()
    {
        var command = new UpdatePaymentCommand { PaymentId = Guid.NewGuid(), Amount = PaymentConstants.MaxAmount + 1 };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Amount);
    }

    [Fact]
    public void Should_Have_Error_When_PaymentMethodId_Is_Invalid()
    {
        var command = new UpdatePaymentCommand { PaymentId = Guid.NewGuid(), PaymentMethodId = (PaymentMethodEnum)999 };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.PaymentMethodId);
    }

    [Fact]
    public void Should_Have_Error_When_Status_Is_Invalid()
    {
        var command = new UpdatePaymentCommand { PaymentId = Guid.NewGuid(), Status = (PaymentStatusEnum)999 };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Status);
    }

    [Fact]
    public void Should_Have_Error_When_TransactionTime_Is_In_The_Future()
    {
        var command = new UpdatePaymentCommand { PaymentId = Guid.NewGuid(), TransactionTime = DateTime.UtcNow.AddMinutes(1) };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.TransactionTime);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        var command = new UpdatePaymentCommand
        {
            PaymentId = Guid.NewGuid(),
            Amount = 100,
            PaymentMethodId = PaymentMethodEnum.Cash,
            Status = PaymentStatusEnum.Completed,
            TransactionTime = DateTime.UtcNow.AddMinutes(-1)
        };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
