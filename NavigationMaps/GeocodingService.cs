using GoogleApi;
using GoogleApi.Entities.Maps.Geocoding.Address.Request;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NavigationMaps.Domain.Interfaces;
using NavigationMaps.Infrastructure.Settings;
using System.Collections.Concurrent;
using GoogleApi.Entities.Common;

namespace NavigationMaps.Infrastructure.Services
{
    public class GeocodingService : IGeocodingService
    {
        private readonly string _googleApiKey;
        private readonly ILogger<GeocodingService> _logger;
        private static readonly ConcurrentDictionary<string, Coordinate> _geocodeCache = new();

        public GeocodingService(IOptions<GoogleMapsSettings> settings, ILogger<GeocodingService> logger)
        {
            _googleApiKey = settings.Value.GoogleApiKey;
            _logger = logger;
        }

        public async Task<Coordinate?> GetCoordinatesAsync(string address)
        {
            if (_geocodeCache.TryGetValue(address, out var cachedLocation))
            {
                return cachedLocation;
            }

            var geocodingRequest = new AddressGeocodeRequest
            {
                Key = _googleApiKey,
                Address = address
            };

            var geocodingResponse = await GoogleMaps.Geocode.AddressGeocode.QueryAsync(geocodingRequest);

            if (geocodingResponse.Results.Any())
            {
                var location = geocodingResponse.Results.First().Geometry.Location;
                var coordinate = new Coordinate(location.Latitude, location.Longitude);

                _geocodeCache[address] = coordinate;
                return coordinate;
            }

            _logger.LogWarning($"Geocoding failed for address: {address}");
            return null;
        }

        public async Task<string?> ReverseGeocodeAsync(double latitude, double longitude)
        {
            var request = new GoogleApi.Entities.Maps.Geocoding.Location.Request.LocationGeocodeRequest
            {
                Key = _googleApiKey,
                Location = new Coordinate(latitude, longitude)
            };

            var response = await GoogleMaps.Geocode.LocationGeocode.QueryAsync(request);

            if (response.Results.Any())
            {
                return response.Results.First().FormattedAddress;
            }

            _logger.LogWarning($"Reverse geocoding failed for coordinates: {latitude}, {longitude}");
            return null;
        }
    }
}
