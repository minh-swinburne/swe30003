namespace SmartRide.Domain.Interfaces;

public interface IMapService
{
    Task<(double Latitude, double Longitude)> GetCoordinatesAsync(string address);
    double CalculateDistance(double latitude1, double longitude1, double latitude2, double longitude2);
    decimal CalculateFare(double distanceInKm);
    int EstimatePickupTime(double distanceInKm);
    int EstimateTravelTime(double distanceInKm);
}
