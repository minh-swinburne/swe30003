namespace SmartRide.Domain.Interfaces;

public interface IMapService
{
    Task<(double DistanceInKm, int EstimatedTimeInMinutes)> CalculateDistanceAndTimeAsync(
        double originLatitude,
        double originLongitude,
        double destinationLatitude,
        double destinationLongitude
    );

    Task<(double Latitude, double Longitude)> GetCoordinatesAsync(string address);
    Task<string> GetAddressAsync(double latitude, double longitude);
}
