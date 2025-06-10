using SmartRide.Web.Models;
using SmartRide.Web.Services.Interfaces;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace SmartRide.Web.Services
{
    public class TokenService(ILocalStorageService localStorage, HttpClient httpClient) : ITokenService
    {
        private readonly ILocalStorageService _localStorage = localStorage;
        private readonly HttpClient _httpClient = httpClient;
        private const string TokenKey = "access_token";

        public async Task<string> GetTokenAsync()
        {
            return await _localStorage.GetItemAsStringAsync(TokenKey);
        }

        public async Task SetTokenAsync(string token)
        {
            await _localStorage.SetItemAsStringAsync(TokenKey, token);

            // Set the token in the HttpClient's default headers
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task RemoveTokenAsync()
        {
            await _localStorage.RemoveItemAsync(TokenKey);

            // Remove the token from the HttpClient's default headers
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<bool> IsTokenValidAsync()
        {
            var token = await GetTokenAsync();

            if (string.IsNullOrEmpty(token))
                return false;

            return await ValidateTokenAsync(token);
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

                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<object>>>(); // if you don't care about parsing claims

                // Check if Data is not null (means token is valid)
                return apiResponse?.Data != null;
            }
            catch
            {
                return false;
            }
        }
    }
}
