using SmartRide.Domain.Enums;

namespace SmartRide.Domain.Interfaces;

public interface ITransactionService
{
    Task<bool> CreateTransactionAsync(PaymentMethodEnum paymentMethod, decimal amount, string currency);
    Task<bool> CaptureTransactionAsync(PaymentMethodEnum paymentMethod, string transactionId);
    Task<bool> RefundTransactionAsync(PaymentMethodEnum paymentMethod, string transactionId);
}
