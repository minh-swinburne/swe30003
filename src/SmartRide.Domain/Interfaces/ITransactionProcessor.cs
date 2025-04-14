using SmartRide.Domain.Enums;

namespace SmartRide.Domain.Interfaces;

public interface ITransactionProcessor
{
    PaymentMethodEnum PaymentMethod { get; }
    Task<string> CreateTransactionAsync(decimal amount, string currency);
    Task<bool> CaptureTransactionAsync(string transactionId);
    Task<bool> RefundTransactionAsync(string transactionId);
}
