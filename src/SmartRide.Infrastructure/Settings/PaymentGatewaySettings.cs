using SmartRide.Infrastructure.Services.Payment;

namespace SmartRide.Infrastructure.Settings;

public class PaymentGatewaySettings
{
    public required PayPalSettings PayPal { get; set; }
}

public class PayPalSettings
{
    public required string ApiEndPoint { get; set; }
    public required string ClientId { get; set; }
    public required string ClientSecret { get; set; }
}


