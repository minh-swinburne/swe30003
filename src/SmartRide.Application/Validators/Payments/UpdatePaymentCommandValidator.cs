using FluentValidation;
using SmartRide.Application.Commands.Payments;
using SmartRide.Common.Constants;
using SmartRide.Common.Responses.Errors;

namespace SmartRide.Application.Validators.Payments;

public class UpdatePaymentCommandValidator : AbstractValidator<UpdatePaymentCommand>
{
    public UpdatePaymentCommandValidator()
    {
        RuleFor(x => x.PaymentId)
            .NotEmpty()
            .WithMessage(PaymentErrors.ID_EMPTY.Message)
            .WithErrorCode(PaymentErrors.ID_EMPTY.Code);

        RuleFor(x => x.Amount)
            .InclusiveBetween(PaymentConstants.MinAmount, PaymentConstants.MaxAmount)
            .WithMessage(PaymentErrors.AMOUNT_INVALID.Message)
            .WithErrorCode(PaymentErrors.AMOUNT_INVALID.Code)
            .When(x => x.Amount.HasValue);

        RuleFor(x => x.PaymentMethodId)
            .IsInEnum()
            .WithMessage(PaymentErrors.METHOD_INVALID.Message)
            .WithErrorCode(PaymentErrors.METHOD_INVALID.Code)
            .When(x => x.PaymentMethodId.HasValue);

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage(PaymentErrors.STATUS_INVALID.Message)
            .WithErrorCode(PaymentErrors.STATUS_INVALID.Code)
            .When(x => x.Status.HasValue);

        RuleFor(x => x.TransactionTime)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage(PaymentErrors.TRANSACTION_TIME_IN_FUTURE.Message)
            .WithErrorCode(PaymentErrors.TRANSACTION_TIME_IN_FUTURE.Code)
            .When(x => x.TransactionTime.HasValue);
    }
}
