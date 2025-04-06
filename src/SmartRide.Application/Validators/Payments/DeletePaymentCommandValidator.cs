using FluentValidation;
using SmartRide.Application.Commands.Payments;
using SmartRide.Common.Responses.Errors;

namespace SmartRide.Application.Validators.Payments;

public class DeletePaymentCommandValidator : AbstractValidator<DeletePaymentCommand>
{
    public DeletePaymentCommandValidator()
    {
        RuleFor(x => x.PaymentId)
            .NotEmpty()
            .WithMessage(PaymentErrors.ID_EMPTY.Message)
            .WithErrorCode(PaymentErrors.ID_EMPTY.Code);
    }
}
