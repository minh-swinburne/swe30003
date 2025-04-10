using System.Threading.Tasks;

namespace SmartRide.Web.Services.Interfaces
{
    public interface ITokenService
    {
        Task<string> GetTokenAsync();
        Task SetTokenAsync(string token);
        Task RemoveTokenAsync();
        Task<bool> IsTokenValidAsync();
        Task<bool> ValidateTokenAsync(string token);
    }
}
