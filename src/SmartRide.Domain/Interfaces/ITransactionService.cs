using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Enums;

namespace SmartRide.Domain.Interfaces;

public interface ITransactionService
{
    Task<bool> CreateTransactionAsync(PaymentMethodEnum paymentMethod, decimal amount, string currency);
    Task<bool> CaptureTransactionAsync(string transactionId);
    Task<bool> RefundTransactionAsync(string transactionId);
}
