using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SmartRide.Domain.Entities;
using SmartRide.Domain.Entities.Join;
using SmartRide.Domain.Entities.Lookup;
using SmartRide.Domain.Enums;
using SmartRide.Infrastructure.Persistence;
using SmartRide.Infrastructure.Settings;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SmartRide.UnitTests.Infrastructure.Persistence;

public class RepositoryTests : IDisposable
{
    private readonly SmartRideDbContext _context;
    private readonly BaseRepository<User> _userRepository;
    private readonly BaseRepository<Vehicle> _vehicleRepository;

    public RepositoryTests()
    {
        var dbSettings = new DbSettings
        {
            Provider = DbProvider.InMemory,
            ConnectionString = "DataSource=:memory:",
            UseSnakeCaseNaming = true
        };
        var options = new DbContextOptionsBuilder<SmartRideDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Unique database name
            .Options;

        _context = new SmartRideDbContext(options, Options.Create(dbSettings));
        _userRepository = new BaseRepository<User>(_context);
        _vehicleRepository = new BaseRepository<Vehicle>(_context);

        // Seed the database with initial data
        SeedDatabase();
    }

    private void SeedDatabase()
    {
        var role = new Role
        {
            Id = RoleEnum.Driver,
            Name = "Driver",
            Description = "A driver in the ride-sharing application."
        };
        var user = new User
        {
            Id = Guid.Parse("12345678-abcd-1234-ef12-34567890abcd"),
            FirstName = "John",
            Email = "john.doe@example.com",
            Phone = "1234567890"
        };
        var userRole = new UserRole
        {
            UserId = user.Id,
            RoleId = role.Id,
            User = user,
            Role = role
        };
        var vehicleType = new VehicleType
        {
            Id = VehicleTypeEnum.SmallCar,
            Name = "Small Car",
            Description = "A small car suitable for city driving.",
            Capacity = 4
        };

        _context.Add(role);
        _context.Add(user);
        _context.Add(userRole);
        _context.Add(vehicleType);
        _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose(); // Ensure DbContext is disposed after each test
    }

    [Fact]
    public async Task Can_Create_Entity()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = "Jane",
            Email = "jane.doe@example.com",
            Phone = "0987654321"
        };

        // Act
        var createdUser = await _userRepository.CreateAsync(user);

        // Assert
        Assert.NotNull(createdUser);
        Assert.Equal(user.Id, createdUser.Id);
    }

    [Fact]
    public async Task Can_Read_Entity()
    {
        // Arrange
        var userId = Guid.Parse("12345678-abcd-1234-ef12-34567890abcd");

        // Act
        var retrievedUser = await _userRepository.GetByIdAsync(userId);

        // Assert
        Assert.NotNull(retrievedUser);
        Assert.Equal(userId, retrievedUser.Id);
        Assert.Equal("John", retrievedUser.FirstName);
        Assert.Equal("john.doe@example.com", retrievedUser.Email);
        Assert.Equal("1234567890", retrievedUser.Phone);
    }

    [Fact]
    public async Task Can_Update_Entity()
    {
        // Arrange
        var newFirstName = "John Updated";
        var user = await _userRepository.GetByIdAsync(Guid.Parse("12345678-abcd-1234-ef12-34567890abcd"));

        // Act
        user!.FirstName = newFirstName;
        var updatedUser = await _userRepository.UpdateAsync(user);

        // Assert
        Assert.NotNull(updatedUser);
        Assert.Equal("John Updated", updatedUser.FirstName);
    }

    [Fact]
    public async Task Can_Delete_Entity()
    {
        // Arrange
        var userId = Guid.Parse("12345678-abcd-1234-ef12-34567890abcd");

        // Act
        var deletedUser = await _userRepository.DeleteAsync(userId);

        // Assert
        Assert.NotNull(deletedUser);
        Assert.Equal(userId, deletedUser.Id);
        Assert.False(await _userRepository.ExistsAsync(userId));
    }

    [Fact]
    public async Task Can_Create_Vehicle_With_UserId_And_VehicleTypeId()
    {
        // Arrange
        var userId = Guid.Parse("12345678-abcd-1234-ef12-34567890abcd");
        var vehicleTypeId = VehicleTypeEnum.SmallCar;
        var vehicle = new Vehicle
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            VehicleTypeId = vehicleTypeId,
            Vin = "1HGCM82633A123456",
            Plate = "ABC123",
            Make = "Toyota",
            Model = "Corolla",
            Year = 2020,
            RegisteredDate = DateTime.UtcNow
        };

        // Act
        await _context.AddAsync(vehicle);
        await _context.SaveChangesAsync();
        var createdVehicle = await _vehicleRepository.GetByIdAsync(vehicle.Id);

        // Assert
        Assert.NotNull(createdVehicle);
        Assert.Equal(userId, createdVehicle.UserId);
        Assert.Equal(userId, createdVehicle.User.Id);
        Assert.Equal(vehicleTypeId, createdVehicle.VehicleTypeId);
        Assert.Equal(vehicleTypeId, createdVehicle.VehicleType.Id);
    }
}
