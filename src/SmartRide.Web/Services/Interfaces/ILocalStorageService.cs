using System.Threading.Tasks;

namespace SmartRide.Web.Services.Interfaces
{
    public interface ILocalStorageService
    {
        Task<T> GetItemAsync<T>(string key);
        Task<string> GetItemAsStringAsync(string key);
        Task SetItemAsync<T>(string key, T value);
        Task SetItemAsStringAsync(string key, string value);
        Task RemoveItemAsync(string key);
        Task ClearAsync();
    }
}
