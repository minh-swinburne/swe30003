using SmartRide.Domain.Enums;
using SmartRide.Common.Interfaces;

namespace SmartRide.Application.DTOs.Payments;

public class ListPaymentRequestDTO : BaseRequestDTO, ISortable, IPageable
{
    public PaymentStatusEnum? Status { get; init; }
    public PaymentMethodEnum? PaymentMethodId { get; init; }
    public DateTime? TransactionTimeFrom { get; init; }
    public DateTime? TransactionTimeTo { get; init; }

    public string? OrderBy { get; set; }
    public bool Ascending { get; set; } = true;
    public int PageSize { get; set; } = 10;
    public int PageNo { get; set; } = 1;
}
