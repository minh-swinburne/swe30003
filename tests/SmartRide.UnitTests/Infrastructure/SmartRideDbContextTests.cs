using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SmartRide.Infrastructure.Persistence;
using SmartRide.Infrastructure.Settings;

namespace SmartRide.UnitTests.Infrastructure;

public class SmartRideDbContextTests
{
    private static DbSettings GetTestDbSettings()
    {
        return new DbSettings
        {
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

        return new SmartRideDbContext(options, Options.Create(dbSettings));
    }

    [Fact]
    public void Can_Create_DbContext_Instance()
    {
        // Act
        var context = CreateDbContext();

        // Assert
        Assert.NotNull(context);
    }
}
