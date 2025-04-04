using Microsoft.Extensions.DependencyInjection;
using NavigationMaps.Domain.Interfaces;
using NavigationMaps.Infrastructure.Persistence;
using NavigationMaps.Infrastructure.Settings;

var serviceProvider = new ServiceCollection()
    .Configure<GoogleMapsSettings>(options =>
    {
        options.GoogleApiKey = "AIzaSyB9sfiCZEK2giKyUpEGRBwQSkYkWMBovnQ";
    })
    .AddScoped<INavigation, NavigationMap>()
    .AddScoped<NavigationService>()
    .BuildServiceProvider();

var navigationService = serviceProvider.GetRequiredService<NavigationService>();

Console.WriteLine("Enter the origin address:");
string originAddress = Console.ReadLine();

Console.WriteLine("Enter the destination address:");
string destinationAddress = Console.ReadLine();

try
{
    var (distance, duration) = await navigationService.GetDistanceAndTimeBetweenAddresses(originAddress, destinationAddress);
    Console.WriteLine($"Distance: {distance} km, Duration: {duration}");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
