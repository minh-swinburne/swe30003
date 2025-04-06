using SmartRide.Domain.Enums;

namespace SmartRide.Application.DTOs.Lookup;

public class PaymentMethodDTO
{
    public PaymentMethodEnum PaymentMethodId { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public bool IsEnabled { get; init; } = true;
}
