using FluentValidation;
using SmartRide.Application.Commands.Payments;
using SmartRide.Common.Constants;
using SmartRide.Common.Responses.Errors;

namespace SmartRide.Application.Validators.Payments;

public class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
{
    public CreatePaymentCommandValidator()
    {
        RuleFor(x => x.RideId)
            .NotEmpty()
            .WithMessage(PaymentErrors.RIDE_ID_EMPTY.Message)
            .WithErrorCode(PaymentErrors.RIDE_ID_EMPTY.Code);

        RuleFor(x => x.Amount)
            .InclusiveBetween(PaymentConstants.MinAmount, PaymentConstants.MaxAmount)
            .WithMessage(PaymentErrors.AMOUNT_INVALID.Message)
            .WithErrorCode(PaymentErrors.AMOUNT_INVALID.Code);

        RuleFor(x => x.PaymentMethodId)
            .IsInEnum()
            .WithMessage(PaymentErrors.METHOD_INVALID.Message)
            .WithErrorCode(PaymentErrors.METHOD_INVALID.Code);
    }
}
