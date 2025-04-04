using GoogleApi;
using GoogleApi.Entities.Maps.DistanceMatrix.Request;
using GoogleApi.Entities.Maps.Common;
using Microsoft.Extensions.Logging;
using NavigationMaps.Domain.Interfaces;
using Microsoft.Extensions.Options;
using NavigationMaps.Infrastructure.Settings;

namespace NavigationMaps.Infrastructure.Persistence
{
    public class NavigationMap : INavigation
    {
        private readonly string _googleApiKey;
        private readonly ILogger<NavigationMap> _logger;
        private readonly IGeocodingService _geocodingService;

        public NavigationMap(IOptions<GoogleMapsSettings> settings, ILogger<NavigationMap> logger, IGeocodingService geocodingService)
        {
            _googleApiKey = settings.Value.GoogleApiKey;
            _logger = logger;
            _geocodingService = geocodingService;
        }

        public async Task<double> CalculateDistanceAsync(string originAddress, string destinationAddress)
        {
            var (distance, _) = await CalculateDistanceAndTimeAsync(originAddress, destinationAddress);
            return distance;
        }

        public async Task<(double distance, TimeSpan duration)> CalculateDistanceAndTimeAsync(string originAddress, string destinationAddress)
        {
            var originCoordinates = await _geocodingService.GetCoordinatesAsync(originAddress);
            var destinationCoordinates = await _geocodingService.GetCoordinatesAsync(destinationAddress);
            if (originCoordinates == null || destinationCoordinates == null)
            {
                _logger.LogError("Failed to fetch coordinates for origin or destination.");
                throw new GeocodingException("Unable to geocode one or both addresses.");
            }
            var request = new DistanceMatrixRequest
            {
                Key = _googleApiKey,
                Origins = new List<Location> { originCoordinates },
                Destinations = new List<Location> { destinationCoordinates },
            };
            var response = await GoogleMaps.DistanceMatrix.QueryAsync(request);
            if (response.Rows.Any() && response.Rows.First().Elements.Any())
            {
                var element = response.Rows.First().Elements.First();
                double distance = element.Distance.Value / 1000.0;
                TimeSpan duration = TimeSpan.FromSeconds(element.Duration.Value);
                return (distance, duration);
            }
            _logger.LogError("Distance matrix API response did not contain valid data.");
            throw new DistanceCalculationException("Unable to calculate distance and time.");
        }

        public class GeocodingException : Exception
        {
            public GeocodingException(string message) : base(message) { }
        }

        public class DistanceCalculationException : Exception
        {
            public DistanceCalculationException(string message) : base(message) { }
        }
    }
}
