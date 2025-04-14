using SmartRide.Application.DTOs.Payments;
using SmartRide.Common.Interfaces;
using SmartRide.Domain.Enums;

namespace SmartRide.Application.Queries.Payments;

public class ListPaymentQuery : BaseQuery<List<ListPaymentResponseDTO>>, ISortable, IPageable
{
    public Guid? PassengerId { get; init; }
    public Guid? DriverId { get; init; }
    public PaymentStatusEnum? Status { get; set; }
    public PaymentMethodEnum? PaymentMethodId { get; set; }
    public DateTime? TransactionTimeFrom { get; set; }
    public DateTime? TransactionTimeTo { get; set; }

    public string? OrderBy { get; set; }
    public bool Ascending { get; set; } = true;
    public int PageSize { get; set; } = 10;
    public int PageNo { get; set; } = 1;
}
