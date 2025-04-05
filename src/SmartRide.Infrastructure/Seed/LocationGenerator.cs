using Bogus;
using SmartRide.Domain.Entities.Base;

namespace SmartRide.Infrastructure.Seed;

public static class LocationGenerator
{
    public static List<Location> GenerateLocations(List<User> users, int maxLocationsPerUser = 3)
    {
        var locations = new List<Location>();
        var faker = new Faker();

        foreach (var user in users)
        {
            // Generate a random number of locations for each user
            int locationCount = faker.Random.Int(1, maxLocationsPerUser);

            for (int i = 0; i < locationCount; i++)
            {
                locations.Add(new Location
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    Address = faker.Address.FullAddress(),
                    Latitude = faker.Address.Latitude(),
                    Longitude = faker.Address.Longitude(),
                    User = user
                });
            }
        }

        return locations;
    }
}
