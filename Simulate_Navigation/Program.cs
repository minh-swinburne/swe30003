using Newtonsoft.Json;

// Domain Layer
public class Location
{
    public string Name { get; set; }
    public string Address { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

public static class DistanceCalculator
{
    private const double EarthRadiusKm = 6371;
    public static double CalculateDistance(Location loc1, Location loc2)
    {
        double lat1 = ToRadians(loc1.Latitude);
        double lon1 = ToRadians(loc1.Longitude);
        double lat2 = ToRadians(loc2.Latitude);
        double lon2 = ToRadians(loc2.Longitude);

        double dLat = lat2 - lat1;
        double dLon = lon2 - lon1;

        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Cos(lat1) * Math.Cos(lat2) *
                   Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return EarthRadiusKm * c;
    }
    private static double ToRadians(double degrees) => degrees * Math.PI / 180;
}

// Infrastructure Layer
public class LocationRepository
{
    private readonly string _filePath;
    public LocationRepository(string filePath) => _filePath = filePath;
    public List<Location> LoadLocations()
    {
        if (!File.Exists(_filePath))
        {
            throw new FileNotFoundException("JSON file not found!");
        }
        string json = File.ReadAllText(_filePath);
        return JsonConvert.DeserializeObject<List<Location>>(json) ?? new List<Location>();
    }
}

// Application Layer
public class DistanceService
{
    private const double SpeedKmPerHour = 50.0; // Assume driver speed is 50 km/h
    public double GetEstimatedTime(Location start, Location end)
    {
        double distance = DistanceCalculator.CalculateDistance(start, end);
        return distance / SpeedKmPerHour * 60; // Time in minutes
    }
}

// Presentation Layer
class Program
{
    static void Main()
    {
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "D:\\Computer Science\\SWE30009-Software architechture and Design\\swe30003\\MapServices\\locations.json");
        var repository = new LocationRepository(filePath);
        var service = new DistanceService();

        List<Location> locations = repository.LoadLocations();

        Console.Write("Enter your address: ");
        string userAddress = Console.ReadLine();

        Location userLocation = locations.FirstOrDefault(loc => loc.Address.Equals(userAddress, StringComparison.OrdinalIgnoreCase));
        if (userLocation == null)
        {
            Console.WriteLine("Address not found in database.");
            return;
        }

        Console.Write("Enter your destination address: ");
        string destinationAddress = Console.ReadLine();

        Location destinationLocation = locations.FirstOrDefault(loc => loc.Address.Equals(destinationAddress, StringComparison.OrdinalIgnoreCase));
        if (destinationLocation == null)
        {
            Console.WriteLine("Destination address not found in database.");
            return;
        }

        Location driverLocation = locations.FirstOrDefault(loc => loc.Name.Equals("Driver Location", StringComparison.OrdinalIgnoreCase));
        if (driverLocation == null)
        {
            Console.WriteLine("Driver location not found in database.");
            return;
        }

        double userToDestinationDistance = DistanceCalculator.CalculateDistance(userLocation, destinationLocation);
        double userToDestinationTime = service.GetEstimatedTime(userLocation, destinationLocation);

        double driverToUserDistance = DistanceCalculator.CalculateDistance(driverLocation, userLocation);
        double driverToUserTime = service.GetEstimatedTime(driverLocation, userLocation);

        Console.WriteLine($"Distance from you to destination: {userToDestinationDistance:F2} km");
        Console.WriteLine($"Estimated Time to destination: {userToDestinationTime:F2} minutes");
        Console.WriteLine($"Distance from driver to you: {driverToUserDistance:F2} km");
        Console.WriteLine($"Estimated Time for driver to reach you: {driverToUserTime:F2} minutes");
    }
}
