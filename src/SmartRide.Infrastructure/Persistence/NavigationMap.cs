using SmartRide.Domain.Interfaces;

namespace SmartRide.Infrastructure.Persistence
{
    public class NavigationMap : INavigation
    {
        public Task<double> CalculateDistanceAsync(string originAddress, string destinationAddress)
        {
            // Simulate distance calculation
            var random = new Random();
            double simulatedDistance = random.Next(1, 1000); // Simulate a distance between 1 and 1000 km
            return Task.FromResult(simulatedDistance);
        }
    }
}