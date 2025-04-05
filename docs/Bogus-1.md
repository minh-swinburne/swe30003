# Question

1. How to use the same Guid for the User Ids (Id, Identity.UserId, UserRoles.UserId...)? How to ensure Identity.LegalName is the same as FirstName + LastName
2. I want only about 75% users have identity, rest is null.
3. How to add bias to PickRandom? for example, for Identity.Status I want 20% pending, 70% approved and 10% rejected.
4. I want that UserRoles always have passenger role, but only about 50% have another role, 45% being Driver, 5% being Manager.

---

# Answer

Here is how you can address each of your requirements step-by-step:

1. **Use the same `Guid` for `User.Id`, `Identity.UserId`, and `UserRoles.UserId`:** Generate the `Guid` once and reuse it across all relevant fields.
2. **Ensure `Identity.LegalName` is the same as `FirstName + LastName`:** Concatenate `FirstName` and `LastName` when setting `Identity.LegalName`.
3. **75% of users have `Identity`, rest is `null`:** Use a conditional statement with a probability check to decide whether to assign an `Identity` or leave it as `null`.
4. **Add bias to `PickRandom` for `Identity.Status`:** Use `PickRandom` with weighted probabilities.
5. **Ensure `UserRoles` always includes the `Passenger` role, and 50% of users have an additional role with specific probabilities for `Driver` and `Manager`:** Use a conditional statement with weighted probabilities to add the second role.

### [UserGenerator.cs](file:///c%3A/Users/xmas4/OneDrive%20-%20Swinburne%20University/BA-CS/SWE30003%20-%20Software%20Architectures%20and%20Design/swe30003/src/SmartRide.Infrastructure/Seed/UserGenerator.cs)

````csharp
public static List<User> GenerateUsers(int count)
{
    var faker = new Faker<User>()
        .RuleFor(u => u.Id, f => Guid.NewGuid())
        .RuleFor(u => u.FirstName, f => f.Name.FirstName())
        .RuleFor(u => u.LastName, f => f.Name.LastName())
        .RuleFor(u => u.Email, f => f.Internet.Email())
        .RuleFor(u => u.Phone, f => f.Phone.PhoneNumber())
        .RuleFor(u => u.Password, f => f.Internet.Password())
        .RuleFor(u => u.Picture, f => f.Internet.Avatar())
        .RuleFor(u =>
        {
            var userId = Guid.NewGuid(); // Generate a single Guid for reuse
            u.Id = userId;
            u.Identity = f.Random.Bool(0.75f) // 75% chance to have Identity
                ? new Identity
                {
                    Id = Guid.NewGuid(),
                    UserId = userId, // Reuse the same Guid
                    Status = f.PickRandom(new[]
                    {
                        IdentityStatusEnum.Pending,
                        IdentityStatusEnum.Approved,
                        IdentityStatusEnum.Rejected
                    }, new[] { 0.2f, 0.7f, 0.1f }), // Weighted probabilities
                    LegalName = $"{u.FirstName} {u.LastName}", // Concatenate FirstName and LastName
                    Sex = f.PickRandom<IdentitySexEnum>(),
                    BirthDate = f.Date.Past(20, DateTime.UtcNow.AddYears(-18)),
                    NationalId = f.Random.Decimal(1000000000, 9999999999).ToString(),
                    Nationality = f.Address.Country(),
                    Address = f.Address.FullAddress(),
                    City = f.Address.City()
                }
                : null; // 25% chance to have null Identity
        })
        .RuleFor(u => u.UserRoles, f =>
        {
            var roles = new List<UserRole>
            {
                new() { RoleId = RoleEnum.Passenger, UserId = u.Id } // Always include Passenger role
            };

            if (f.Random.Bool(0.5f)) // 50% chance to have an additional role
            {
                roles.Add(new UserRole
                {
                    RoleId = f.PickRandom(new[] { RoleEnum.Driver, RoleEnum.Manager }, new[] { 0.9f, 0.1f }), // Weighted probabilities
                    UserId = u.Id // Reuse the same Guid
                });
            }

            return roles;
        });

    return faker.Generate(count);
}
````

### Summary of Changes:
1. Added a reusable `Guid` for `User.Id`, `Identity.UserId`, and `UserRoles.UserId`.
2. Ensured `Identity.LegalName` is set to `FirstName + LastName`.
3. Used a 75% probability for assigning `Identity`, leaving it `null` otherwise.
4. Added weighted probabilities for `Identity.Status` and the second role in `UserRoles`.

Made changes.