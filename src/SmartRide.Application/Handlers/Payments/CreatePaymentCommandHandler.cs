using AutoMapper;
using SmartRide.Application.Commands.Payments;
using SmartRide.Application.DTOs.Payments;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Application.Handlers.Payments;

public class CreatePaymentCommandHandler(IRepository<Payment> paymentRepository, IMapper mapper)
    : BaseCommandHandler<CreatePaymentCommand, CreatePaymentResponseDTO>
{
    private readonly IRepository<Payment> _paymentRepository = paymentRepository;
    private readonly IMapper _mapper = mapper;

    public override async Task<CreatePaymentResponseDTO> Handle(CreatePaymentCommand command, CancellationToken cancellationToken)
    {
        var payment = _mapper.Map<Payment>(command);
        var createdPayment = await _paymentRepository.CreateAsync(payment, cancellationToken);
        return _mapper.Map<CreatePaymentResponseDTO>(createdPayment);
    }
}
