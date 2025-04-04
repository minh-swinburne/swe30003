namespace SmartRide.Domain.Interfaces
{
    public interface INavigation
    {
        Task<double> CalculateDistanceAsync(string originAddress, string destinationAddress);
    }
}