using Bogus;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Enums;

namespace SmartRide.Infrastructure.Seed;

public static class VehicleGenerator
{
    public static List<Vehicle> GenerateVehicles(List<User> users, int maxVehiclesPerDriver = 3)
    {
        var vehicles = new List<Vehicle>();
        var faker = new Faker();

        // Filter drivers from the user list
        var drivers = users.Where(u => u.IsDriver()).ToList();

        foreach (var driver in drivers)
        {
            // Generate a random number of vehicles for each driver
            int vehicleCount = faker.Random.Int(1, maxVehiclesPerDriver);

            for (int i = 0; i < vehicleCount; i++)
            {
                var vehicleType = faker.PickRandom<VehicleTypeEnum>();

                vehicles.Add(new Vehicle
                {
                    Id = Guid.NewGuid(),
                    UserId = driver.Id,
                    VehicleTypeId = vehicleType,
                    Vin = faker.Vehicle.Vin(),
                    Plate = faker.Random.String2(6, "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"),
                    Make = faker.Vehicle.Manufacturer(),
                    Model = faker.Vehicle.Model(),
                    Year = faker.Date.Past(10, DateTime.UtcNow).Year,
                    RegisteredDate = faker.Date.Past(5, DateTime.UtcNow)
                });
            }
        }

        return vehicles;
    }
}
