using GoogleApi.Entities.Common;

namespace NavigationMaps.Domain.Interfaces
{
    public interface INavigation
    {
        Task<double> CalculateDistanceAsync(string originAddress, string destinationAddress);
        Task<(double distance, TimeSpan duration)> CalculateDistanceAndTimeAsync(string originAddress, string destinationAddress);
    }

    public interface IGeocodingService
    {
        Task<Coordinate?> GetCoordinatesAsync(string address);
        Task<string?> ReverseGeocodeAsync(double latitude, double longitude);
    }
}