using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PaypalServerSdk.Standard;
using PaypalServerSdk.Standard.Authentication;
using PaypalServerSdk.Standard.Models;
using SmartRide.Common.Exceptions;
using SmartRide.Common.Responses.Errors;
using SmartRide.Domain.Enums;
using SmartRide.Domain.Interfaces;
using SmartRide.Infrastructure.Settings;
using System.Globalization;

namespace SmartRide.Infrastructure.Services.Transaction;

public class PayPalProcessor : ITransactionProcessor
{
    private readonly PaypalServerSdkClient _client;
    private readonly PayPalSettings _paypalSettings; // Access nested settings
    public PaymentMethodEnum PaymentMethod => PaymentMethodEnum.PayPal;

    public PayPalProcessor(IOptions<TransactionSettings> txSettings)
    {
        _paypalSettings = txSettings.Value.PayPal;
        _client = GenerateClient();
    }

    private PaypalServerSdkClient GenerateClient()
    {
        return new PaypalServerSdkClient.Builder()
            .ClientCredentialsAuth(
                new ClientCredentialsAuthModel.Builder(
                    _paypalSettings.ClientId,
                    _paypalSettings.ClientSecret
                )
                .Build()
            )
            .Environment(_paypalSettings.SandboxMode ? PaypalServerSdk.Standard.Environment.Sandbox : PaypalServerSdk.Standard.Environment.Production)
            .LoggingConfig(config => config
                .LogLevel(LogLevel.Information)
                .RequestConfig(reqConfig => reqConfig.Body(true))
                .ResponseConfig(respConfig => respConfig.Headers(true))
            )
            .Build();
    }

    public async Task<(string, string)> CreateTransactionAsync(
        decimal amount,
        string currency,
        RideTypeEnum rideType,
        VehicleTypeEnum vehicleType,
        string pickupAddress,
        string destinationAddress
        )
    {
        var orderRequest = new OrderRequest
        {
            Intent = CheckoutPaymentIntent.Capture,
            PurchaseUnits =
            [
                new PurchaseUnitRequest
                {
                    Amount = new AmountWithBreakdown
                    {
                        CurrencyCode = currency,
                        MValue = amount.ToString("F2", CultureInfo.InvariantCulture),
                        Breakdown = new AmountBreakdown
                        {
                            ItemTotal = new Money
                            {
                                CurrencyCode = currency,
                                MValue = amount.ToString("F2", CultureInfo.InvariantCulture)
                            }
                        }
                    },
                    Items =
                    [
                        new Item
                        {
                            Name = "SmartRide Online Ride-Sharing Service",
                            Description = $"{rideType} ride (by {vehicleType}) from {pickupAddress} to {destinationAddress}.",
                            UnitAmount = new Money
                            {
                                CurrencyCode = currency,
                                MValue = amount.ToString("F2", CultureInfo.InvariantCulture)
                            },
                            Quantity = "1",
                            Category = ItemCategory.DigitalGoods
                        }
                    ]
                }
            ]
        };

        var response = await _client.OrdersController.CreateOrderAsync(new CreateOrderInput { Body = orderRequest });
        if (response.StatusCode == 200 || response.StatusCode == 201)
        {
            return (
                response.Data.Id,
                response.Data.Links
                    .Where(link => link.Rel == "approve")
                    .Select(link => link.Href)
                    .First()
                ); // Return the order ID and approval URL
        }

        throw new BaseException("Payment", PaymentErrors.CREATE_TRANSACTION_FAILED.FormatMessage(("Details", response.ToString()!)));
    }

    public async Task<bool> CaptureTransactionAsync(string transactionId)
    {
        var response = await _client.OrdersController.CaptureOrderAsync(new CaptureOrderInput { Id = transactionId });

        if (response.StatusCode == 200 || response.StatusCode == 201)
        {
            return true;
        }

        throw new BaseException("Payment", PaymentErrors.CAPTURE_TRANSACTION_FAILED.FormatMessage(("Details", response.ToString()!)));
    }

    public async Task<bool> RefundTransactionAsync(string transactionId)
    {
        var response = await _client.PaymentsController.RefundCapturedPaymentAsync(new RefundCapturedPaymentInput { CaptureId = transactionId });
        if (response.StatusCode == 200)
        {
            return true;
        }

        throw new BaseException("Payment", PaymentErrors.REFUND_TRANSACTION_FAILED.FormatMessage(("Details", response.ToString()!)));
    }
}
