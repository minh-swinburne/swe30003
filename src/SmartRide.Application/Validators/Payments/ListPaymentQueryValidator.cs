using SmartRide.Application.Queries.Payments;
using SmartRide.Domain.Entities.Base;

namespace SmartRide.Application.Validators.Payments;

public class ListPaymentQueryValidator : BaseListQueryValidator<ListPaymentQuery, Payment>
{
    public ListPaymentQueryValidator()
    {
        // Additional validation rules for ListPaymentQuery can go here
    }
}
