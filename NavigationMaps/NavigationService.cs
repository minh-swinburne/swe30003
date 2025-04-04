namespace NavigationMaps.Domain.Interfaces
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

        public async Task<(double distance, TimeSpan duration)> GetDistanceAndTimeBetweenAddresses(string origin, string destination)
        {
            return await _navigation.CalculateDistanceAndTimeAsync(origin, destination);
        }
    }
}