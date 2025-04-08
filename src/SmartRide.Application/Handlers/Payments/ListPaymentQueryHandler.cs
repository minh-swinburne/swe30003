using AutoMapper;
using SmartRide.Application.DTOs.Payments;
using SmartRide.Application.Interfaces;
using SmartRide.Application.Queries.Payments;
using SmartRide.Common.Extensions;
using SmartRide.Common.Helpers;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;
using System.Linq.Expressions;

namespace SmartRide.Application.Handlers.Payments;

public class ListPaymentQueryHandler(IRepository<Payment> paymentRepository, IMapper mapper)
    : BaseQueryHandler<ListPaymentQuery, List<ListPaymentResponseDTO>>, IFilterableHandler<ListPaymentQuery, Payment>
{
    private readonly IRepository<Payment> _paymentRepository = paymentRepository;
    private readonly IMapper _mapper = mapper;

    public override async Task<List<ListPaymentResponseDTO>> Handle(ListPaymentQuery query, CancellationToken cancellationToken)
    {
        Expression<Func<Payment, object>>? orderBy = null;
        Expression<Func<Payment, bool>>? filter = BuildFilter(query);

        if (!string.IsNullOrEmpty(query.OrderBy))
        {
            var propertyInfo = QueryHelper.GetProperty<Payment>(query.OrderBy);
            orderBy = QueryHelper.GetSortExpression<Payment>(propertyInfo!);
        }

        var payments = await _paymentRepository.GetWithFilterAsync<ListPaymentResponseDTO>(
            filter,
            orderBy: orderBy,
            ascending: query.Ascending,
            skip: query.PageSize * (query.PageNo - 1),
            limit: query.PageSize,
            includes: ["PaymentMethod"],
            cancellationToken: cancellationToken
        );

        return _mapper.Map<List<ListPaymentResponseDTO>>(payments);
    }

    public Expression<Func<Payment, bool>>? BuildFilter(ListPaymentQuery query)
    {
        Expression<Func<Payment, bool>>? filter = null;

        if (query.Status.HasValue)
            filter = filter.AddFilter(payment => payment.Status == query.Status.Value);

        if (query.PaymentMethodId.HasValue)
            filter = filter.AddFilter(payment => payment.PaymentMethodId == query.PaymentMethodId.Value);

        if (query.TransactionTimeFrom.HasValue)
            filter = filter.AddFilter(payment => payment.TransactionTime >= query.TransactionTimeFrom.Value);

        if (query.TransactionTimeTo.HasValue)
            filter = filter.AddFilter(payment => payment.TransactionTime <= query.TransactionTimeTo.Value);

        return filter;
    }
}
