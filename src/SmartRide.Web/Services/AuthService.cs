using SmartRide.Web.Models;
using SmartRide.Web.Services.Interfaces;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace SmartRide.Web.Services
{
    public class AuthService(HttpClient httpClient, ITokenService tokenService) : IAuthService
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ITokenService _tokenService = tokenService;

        public async Task<string> LoginAsync(string email, string password)
        {
            var loginRequest = new
            {
                Email = email,
                Password = password
            };

            var content = new StringContent(JsonSerializer.Serialize(loginRequest), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("auth/login", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new ApiException(
                    $"Login failed. Status code: {(int)response.StatusCode}",
                    (int)response.StatusCode,
                    errorContent,
                    null!
                );
            }

            // Parse the response which contains a JWT token
            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();

            if (apiResponse == null || string.IsNullOrEmpty(apiResponse.Data))
            {
                throw new ApiException(
                    "Login failed. Invalid response from server.",
                    (int)response.StatusCode,
                    apiResponse?.Info ?? "Unknown error",
                    null!
                );
            }

            // Store the token
            await _tokenService.SetTokenAsync(apiResponse.Data);

            // Return the JWT token
            return apiResponse.Data;
        }

        public async Task<bool> LogoutAsync()
        {
            // First try to call the logout endpoint
            try
            {
                var response = await _httpClient.PostAsync("auth/logout", null);

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();
                    if (apiResponse?.Data == true)
                    {
                        // If successful, remove the token
                        await _tokenService.RemoveTokenAsync();
                        return true;
                    }
                }
            }
            catch
            {
                // If the API call fails, still remove the token locally
            }

            // Always remove the token locally even if the API call fails
            await _tokenService.RemoveTokenAsync();
            return true;
        }

        public async Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword)
        {
            var changePasswordRequest = new
            {
                UserId = userId,
                CurrentPassword = currentPassword,
                NewPassword = newPassword
            };

            var content = new StringContent(JsonSerializer.Serialize(changePasswordRequest), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("auth/change-password", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new ApiException(
                    $"Change password failed. Status code: {(int)response.StatusCode}",
                    (int)response.StatusCode,
                    errorContent,
                    null!
                );
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();

            return apiResponse?.Data ?? false;
        }

        public async Task<CreateUserResponseDTO> RegisterAsync(CreateUserRequestDTO request)
        {
            var response = await _httpClient.PostAsJsonAsync("auth/register", request);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new ApiException(
                    $"Failed to register user. Status code: {(int)response.StatusCode}",
                    (int)response.StatusCode,
                    errorContent,
                    null!
                );
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<CreateUserResponseDTO>>();

            if (apiResponse == null || apiResponse.Data == null)
            {
                throw new ApiException(
                    "Failed to register user. Invalid response from server.",
                    (int)response.StatusCode,
                    apiResponse?.Info ?? "Unknown error",
                    null!
                );
            }

            return apiResponse.Data;
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                var validateRequest = new { Token = token };
                var content = new StringContent(JsonSerializer.Serialize(validateRequest), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("auth/validate", content);

                if (!response.IsSuccessStatusCode)
                    return false;

                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();

                return apiResponse?.Data ?? false;
            }
            catch
            {
                return false;
            }
        }
    }
}
