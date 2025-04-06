using AutoMapper;
using SmartRide.Application.Commands.Payments;
using SmartRide.Application.DTOs.Payments;
using SmartRide.Common.Exceptions;
using SmartRide.Common.Responses.Errors;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Application.Handlers.Payments;

public class UpdatePaymentCommandHandler(IRepository<Payment> paymentRepository, IMapper mapper)
    : BaseCommandHandler<UpdatePaymentCommand, UpdatePaymentResponseDTO>
{
    private readonly IRepository<Payment> _paymentRepository = paymentRepository;
    private readonly IMapper _mapper = mapper;

    public override async Task<UpdatePaymentResponseDTO> Handle(UpdatePaymentCommand command, CancellationToken cancellationToken)
    {
        var payment = await _paymentRepository.GetByIdAsync(command.PaymentId, cancellationToken)
            ?? throw new BaseException(PaymentErrors.Module, PaymentErrors.ID_NOT_FOUND.FormatMessage(("PaymentId", command.PaymentId)));

        _mapper.Map(command, payment);

        var updatedPayment = await _paymentRepository.UpdateAsync(payment, cancellationToken);
        
        return _mapper.Map<UpdatePaymentResponseDTO>(updatedPayment);
    }
}
