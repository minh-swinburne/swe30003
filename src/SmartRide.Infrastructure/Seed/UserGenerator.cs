using Bogus;
using Microsoft.AspNetCore.Identity;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Entities.Join;
using SmartRide.Domain.Enums;

namespace SmartRide.Infrastructure.Seed;

public static class UserGenerator
{
    public static List<User> GenerateUsers(int count, IPasswordHasher<User> passwordHasher)
    {
        var faker = new Faker<User>();
        var usedNationalIds = new HashSet<string>(); // Track used National IDs
        // var passwordHasher = new

        faker.RuleFor(u => u.Id, f => Guid.NewGuid())
            .RuleFor(u => u.FirstName, f => f.Name.FirstName())
            .RuleFor(u => u.LastName, f => f.Name.LastName())
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.Phone, f => f.Phone.PhoneNumber())
            .RuleFor(u => u.Password, (f, u) => passwordHasher.HashPassword(u, f.Internet.Password())) // Hash the password
            .RuleFor(u => u.Picture, f => f.Internet.Avatar())
            .RuleFor(u => u.Identity, (f, u) =>
            {
                if (f.Random.Bool(0.75f)) // 75% chance to have Identity
                {
                    string nationalId;
                    do
                    {
                        nationalId = f.Random.Long(1000000000, 9999999999).ToString();
                    } while (!usedNationalIds.Add(nationalId)); // Ensure uniqueness

                    return new Identity
                    {
                        Id = Guid.NewGuid(),
                        UserId = u.Id,
                        Status = f.Random.WeightedRandom(
                        [   // 70% Approved, 20% Pending, 10% Rejected
                            IdentityStatusEnum.Approved,
                            IdentityStatusEnum.Pending,
                            IdentityStatusEnum.Rejected
                        ], [0.7f, 0.2f, 0.1f]),
                        LegalName = $"{u.FirstName} {u.LastName}",
                        Sex = f.Random.WeightedRandom([
                            IdentitySexEnum.Male,
                            IdentitySexEnum.Female,
                            IdentitySexEnum.Other
                        ], [0.45f, 0.45f, 0.1f]),
                        BirthDate = f.Date.Past(30, DateTime.UtcNow.AddYears(-18)),
                        NationalId = nationalId,
                        Nationality = f.Address.Country(),
                        Address = f.Address.FullAddress(),
                        City = f.Address.City()
                    };
                }
                return null;
            })
            .RuleFor(u => u.UserRoles, (f, u) =>
            {
                var roles = new List<UserRole>
                {
                    new() { RoleId = RoleEnum.Passenger, UserId = u.Id } // Always include Passenger role
                };

                if (f.Random.Bool(0.5f)) // 50% chance to have an additional role
                {
                    roles.Add(new UserRole
                    {
                        RoleId = f.Random.WeightedRandom(
                        [   // 90% Driver, 10% Manager
                            RoleEnum.Driver,
                            RoleEnum.Manager
                        ], [0.9f, 0.1f]),
                        UserId = u.Id
                    });
                }

                return roles;
            });

        return faker.Generate(count);
    }
}