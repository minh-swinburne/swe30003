using Microsoft.Extensions.Options;
using SmartRide.Domain.Enums;
using SmartRide.Domain.Interfaces;
using SmartRide.Infrastructure.Settings;

namespace SmartRide.Infrastructure.Services.Transaction;

public class TransactionService(IOptions<TransactionSettings> settings, IEnumerable<ITransactionProcessor> processors)
    : ITransactionService
{
    private readonly TransactionSettings _settings = settings.Value;
    private readonly IEnumerable<ITransactionProcessor> _processors = processors;

    // Constructor injection for ITransactionProcessor implementations
    // Strategy pattern
    public ITransactionProcessor GetProcessor(PaymentMethodEnum paymentMethod)
    {
        return _processors.FirstOrDefault(p => p.PaymentMethod == paymentMethod)
            ?? throw new NotImplementedException($"Payment method {paymentMethod} is not implemented.");
    }

    public async Task<(string, string)> CreateTransactionAsync(
        PaymentMethodEnum paymentMethod,
        decimal amount,
        string currency,
        RideTypeEnum rideType,
        VehicleTypeEnum vehicleType,
        string pickupAddress,
        string destinationAddress
        )
    {
        var processor = GetProcessor(paymentMethod);
        var (transactionId, approvalUrl) = await processor.CreateTransactionAsync(amount, currency, rideType, vehicleType, pickupAddress, destinationAddress);

        return (transactionId, approvalUrl);
    }

    public async Task<bool> CaptureTransactionAsync(PaymentMethodEnum paymentMethod, string transactionId)
    {
        var processor = GetProcessor(paymentMethod);
        return await processor.CaptureTransactionAsync(transactionId);
    }

    public async Task<bool> RefundTransactionAsync(PaymentMethodEnum paymentMethod, string transactionId)
    {
        var processor = GetProcessor(paymentMethod);
        return await processor.RefundTransactionAsync(transactionId);
    }
}
