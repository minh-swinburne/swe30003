using SmartRide.Application.Commands.Payments;
using SmartRide.Application.DTOs.Payments;
using SmartRide.Common.Exceptions;
using SmartRide.Common.Responses.Errors;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Application.Handlers.Payments;

public class DeletePaymentCommandHandler(IRepository<Payment> paymentRepository)
    : BaseCommandHandler<DeletePaymentCommand, DeletePaymentResponseDTO>
{
    private readonly IRepository<Payment> _paymentRepository = paymentRepository;

    public override async Task<DeletePaymentResponseDTO> Handle(DeletePaymentCommand command, CancellationToken cancellationToken)
    {
        var payment = await _paymentRepository.GetByIdAsync(command.PaymentId, cancellationToken: cancellationToken)
            ?? throw new BaseException(PaymentErrors.Module, PaymentErrors.ID_NOT_FOUND.FormatMessage(("PaymentId", command.PaymentId)));

        await _paymentRepository.DeleteAsync(command.PaymentId, cancellationToken);
        return new DeletePaymentResponseDTO
        {
            PaymentId = payment.Id,
            Success = true
        };
    }
}
