using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using SmartRide.Web.Services.Interfaces;

namespace SmartRide.Web.Services
{
    public class LocalStorageService : ILocalStorageService
    {
        private readonly IJSRuntime _jsRuntime;

        public LocalStorageService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<T> GetItemAsync<T>(string key)
        {
            var json = await _jsRuntime.InvokeAsync<string>("localStorageInterop.getItem", key);

            if (string.IsNullOrEmpty(json))
                return default;

            return JsonSerializer.Deserialize<T>(json);
        }

        public async Task<string> GetItemAsStringAsync(string key)
        {
            return await _jsRuntime.InvokeAsync<string>("localStorageInterop.getItem", key);
        }

        public async Task SetItemAsync<T>(string key, T value)
        {
            await _jsRuntime.InvokeVoidAsync("localStorageInterop.setItem", key, JsonSerializer.Serialize(value));
        }

        public async Task SetItemAsStringAsync(string key, string value)
        {
            await _jsRuntime.InvokeVoidAsync("localStorageInterop.setItem", key, value);
        }

        public async Task RemoveItemAsync(string key)
        {
            await _jsRuntime.InvokeVoidAsync("localStorageInterop.removeItem", key);
        }

        public async Task ClearAsync()
        {
            await _jsRuntime.InvokeVoidAsync("localStorageInterop.clear");
        }
    }
}
