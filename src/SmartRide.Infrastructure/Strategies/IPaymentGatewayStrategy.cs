using System.Threading.Tasks;

namespace SmartRide.Infrastructure.Strategies
{
    public interface IPaymentGatewayStrategy
    {
        Task<bool> ProcessPaymentAsync(decimal amount, string currency);
    }
}
