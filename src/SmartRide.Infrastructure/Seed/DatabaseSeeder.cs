using Bogus;
using Microsoft.AspNetCore.Identity;
using SmartRide.Domain.Entities.Base;
using SmartRide.Infrastructure.Persistence;

namespace SmartRide.Infrastructure.Seed;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(SmartRideDbContext dbContext, IPasswordHasher<User> passwordHasher, string csvFilePath, int userCount = 100, int rideCount = 50)
    {
        if (!dbContext.Set<User>().Any())
        {
            // Set a seed for random generator for reproducibility
            Randomizer.Seed = new Random(1508);
            // Generate users and save credentials to CSV
            var users = UserGenerator.GenerateUsers(userCount, passwordHasher, csvFilePath);
            await dbContext.Set<User>().AddRangeAsync(users);
            await dbContext.SaveChangesAsync();

            // Generate locations
            var locations = LocationGenerator.GenerateLocations(users);
            await dbContext.Set<Location>().AddRangeAsync(locations);
            await dbContext.SaveChangesAsync();

            // Generate vehicles
            var vehicles = VehicleGenerator.GenerateVehicles(users);
            await dbContext.Set<Vehicle>().AddRangeAsync(vehicles);
            await dbContext.SaveChangesAsync();

            // Generate licenses
            var licenses = LicenseGenerator.GenerateLicenses(users);
            await dbContext.Set<License>().AddRangeAsync(licenses);
            await dbContext.SaveChangesAsync();

            // Generate rides
            var rides = RideGenerator.GenerateRides(users, locations, vehicles, rideCount);
            await dbContext.Set<Ride>().AddRangeAsync(rides);
            await dbContext.SaveChangesAsync();
        }
    }
}
