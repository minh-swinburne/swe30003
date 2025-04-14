using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Entities.Lookup;
using SmartRide.Domain.Enums;
using SmartRide.Infrastructure.Persistence;
using SmartRide.Infrastructure.Settings;

namespace SmartRide.UnitTests.Infrastructure.Persistence;

public class DbContextTests
{
    private static readonly Mock<IMediator> _mockMediator = new();

    private static DbSettings GetTestDbSettings()
    {
        return new DbSettings
        {
            Provider = DbProviderEnum.InMemory,
            ConnectionString = "DataSource=:memory:",
            UseSnakeCaseNaming = true
        };
    }

    private static SmartRideDbContext CreateDbContext()
    {
        var dbSettings = GetTestDbSettings();
        var options = new DbContextOptionsBuilder<SmartRideDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        return new SmartRideDbContext(options, Options.Create(dbSettings), _mockMediator.Object);
    }

    [Fact]
    public void Can_Create_DbContext_Instance()
    {
        // Act
        var context = CreateDbContext();

        // Assert
        Assert.NotNull(context);
    }

    [Fact]
    public void New_User_Has_Role_Passenger()
    {
        // Arrange
        var context = CreateDbContext();
        var role = new Role
        {
            Id = RoleEnum.Passenger,
            Name = "Passenger",
            Description = "A passenger in the ride-sharing application."
        };
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = "Jane",
            LastName = "Doe",
            Email = "jane.doe@example.com",
            Phone = "0987654321",
        };

        // Act
        context.Set<Role>().Add(role);
        context.Set<User>().Add(user);
        context.SaveChanges();

        // Assert
        var savedUser = context.Set<User>().Find(user.Id);
        Assert.NotNull(savedUser);
        Assert.Contains(savedUser.UserRoles, ur => ur.RoleId == RoleEnum.Passenger);
        Assert.Contains(savedUser.Roles, r => r.Id == RoleEnum.Passenger);
    }
}
