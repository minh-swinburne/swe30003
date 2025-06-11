using SmartRide.Web.Models;

namespace SmartRide.Web.Services.Interfaces;

public interface IAuthService
{
    // Authentication methods
    Task<string> LoginAsync(string email, string password);
    Task<bool> LogoutAsync();
    Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);
    Task<CreateUserResponseDTO> RegisterAsync(CreateUserRequestDTO request);
    Task<bool> ValidateTokenAsync(string token);
}
