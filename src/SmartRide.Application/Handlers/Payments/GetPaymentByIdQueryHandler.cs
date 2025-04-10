using AutoMapper;
using SmartRide.Application.DTOs.Payments;
using SmartRide.Application.Queries.Payments;
using SmartRide.Common.Exceptions;
using SmartRide.Common.Responses.Errors;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Application.Handlers.Payments;

public class GetPaymentByIdQueryHandler(IRepository<Payment> paymentRepository, IMapper mapper)
    : BaseQueryHandler<GetPaymentByIdQuery, GetPaymentResponseDTO>
{
    private readonly IRepository<Payment> _paymentRepository = paymentRepository;
    private readonly IMapper _mapper = mapper;

    public override async Task<GetPaymentResponseDTO> Handle(GetPaymentByIdQuery query, CancellationToken cancellationToken)
    {
        var payment = await _paymentRepository.GetByIdAsync(
            query.PaymentId,
            [
                p => p.Ride,
                p => p.PaymentMethod
            ],
            cancellationToken
            ) ?? throw new BaseException(PaymentErrors.Module, PaymentErrors.ID_NOT_FOUND.FormatMessage(("PaymentId", query.PaymentId)));

        return _mapper.Map<GetPaymentResponseDTO>(payment);
    }
}
