using Bogus;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Enums;

namespace SmartRide.Infrastructure.Seed;

public static class LicenseGenerator
{
    public static List<License> GenerateLicenses(List<User> users, int maxLicensesPerDriver = 2)
    {
        var licenses = new List<License>();
        var faker = new Faker();
        var usedNumbers = new HashSet<string>(); // Track used license numbers

        // Filter drivers from the user list
        var drivers = users.Where(u => u.IsDriver()).ToList();

        foreach (var driver in drivers)
        {
            // Generate a random number of licenses for each driver
            int licenseCount = faker.Random.Int(1, maxLicensesPerDriver);

            for (int i = 0; i < licenseCount; i++)
            {
                string licenseNumber;
                do
                {
                    licenseNumber = faker.Random.String2(10, "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789");
                } while (!usedNumbers.Add(licenseNumber)); // Ensure uniqueness

                licenses.Add(new License
                {
                    Id = Guid.NewGuid(),
                    UserId = driver.Id,
                    Number = licenseNumber,
                    Type = faker.PickRandom<LicenseTypeEnum>(),
                    Status = faker.PickRandom<LicenseStatusEnum>(),
                    IssuedDate = faker.Date.Past(5, DateTime.UtcNow),
                    IssuedCountry = faker.Address.Country(),
                    User = driver
                });
            }
        }

        return licenses;
    }
}
