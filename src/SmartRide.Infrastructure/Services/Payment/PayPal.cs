using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRide.Infrastructure.Services.Payment;

public class PayPal : BasePaymentGateway
{
    public string ApiEndPoint { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }

    public PayPal(string apiEndPoint, string clientId, string clientSecret)
    {
        ApiEndPoint = apiEndPoint;
        ClientId = clientId;
        ClientSecret = clientSecret;
    }
}
