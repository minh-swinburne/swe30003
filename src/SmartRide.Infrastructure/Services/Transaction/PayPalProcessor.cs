using Microsoft.Extensions.Options;
using SmartRide.Domain.Enums;
using SmartRide.Domain.Interfaces;
using SmartRide.Infrastructure.Settings;

namespace SmartRide.Infrastructure.Services.Transaction;

public class PayPalProcessor(IOptions<TransactionSettings> settings) : ITransactionProcessor
{
    private readonly PayPalSettings _settings = settings.Value.PayPal; // Access nested settings
    public PaymentMethodEnum PaymentMethod => PaymentMethodEnum.PayPal;

    public Task<string> CreateTransactionAsync(decimal amount, string currency)
    {
        // Implement PayPal transaction creation logic here
        throw new NotImplementedException();
    }

    public Task<bool> CaptureTransactionAsync(string transactionId)
    {
        // Implement PayPal transaction capture logic here
        throw new NotImplementedException();
    }

    public Task<bool> RefundTransactionAsync(string transactionId)
    {
        // Implement PayPal transaction refund logic here
        throw new NotImplementedException();
    }
}
