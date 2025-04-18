using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.Options;
using SmartRide.Domain.Interfaces;
using SmartRide.Infrastructure.Settings;

namespace SmartRide.Infrastructure.Services;

public class GoogleMapsService(HttpClient httpClient, IOptions<MapSettings> mapSettings) : IMapService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly string _apiKey = mapSettings.Value.GoogleApiKey;

    public async Task<(double DistanceInKm, int EstimatedTimeInMinutes)> CalculateDistanceAndTimeAsync(
        double originLatitude,
        double originLongitude,
        double destinationLatitude,
        double destinationLongitude
    )
    {
        return (1.5, 10); // Placeholder for actual implementation
        var url = $"https://maps.googleapis.com/maps/api/distancematrix/json?origins={originLatitude},{originLongitude}&destinations={destinationLatitude},{destinationLongitude}&mode=driving&key={_apiKey}";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<GoogleDistanceMatrixResponse>(jsonResponse);

        if (data?.Rows?.FirstOrDefault()?.Elements?.FirstOrDefault()?.Status != "OK")
        {
            throw new Exception("Failed to retrieve distance and time from Google Maps API.");
        }

        var element = data.Rows.First().Elements.First();
        var distanceInKm = element.Distance.Value / 1000.0; // Convert meters to kilometers
        var estimatedTimeInMinutes = element.Duration.Value / 60; // Convert seconds to minutes

        return (distanceInKm, estimatedTimeInMinutes);
    }

    public async Task<(double Latitude, double Longitude)> GetCoordinatesAsync(string address)
    {
        // Implementation for getting coordinates from Google Maps API
        throw new NotImplementedException();
    }

    public async Task<string> GetAddressAsync(double latitude, double longitude)
    {
        // Implementation for getting address from Google Maps API
        throw new NotImplementedException();
    }
}

// Helper classes for deserializing Google Maps API response
public class GoogleDistanceMatrixResponse
{
    public List<Row> Rows { get; set; } = new();
}

public class Row
{
    public List<Element> Elements { get; set; } = new();
}

public class Element
{
    public Distance Distance { get; set; } = new();
    public Duration Duration { get; set; } = new();
    public string Status { get; set; } = string.Empty;
}

public class Distance
{
    public int Value { get; set; } // Distance in meters
}

public class Duration
{
    public int Value { get; set; } // Duration in seconds
}
