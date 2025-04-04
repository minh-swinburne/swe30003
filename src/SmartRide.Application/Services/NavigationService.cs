using SmartRide.Domain.Interfaces;

namespace SmartRide.Application.Services
{
    public class NavigationService
    {
        private readonly INavigation _navigation;

        public NavigationService(INavigation navigation)
        {
            _navigation = navigation;
        }

        public async Task<double> GetDistanceBetweenAddresses(string origin, string destination)
        {
            return await _navigation.CalculateDistanceAsync(origin, destination);
        }
    }
}