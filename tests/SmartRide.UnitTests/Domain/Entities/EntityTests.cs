using Microsoft.EntityFrameworkCore;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Entities.Join;
using SmartRide.Domain.Entities.Lookup;
using SmartRide.Domain.Enums;
using SmartRide.Domain.Events;

namespace SmartRide.UnitTests.Domain.Entities;

public class EntityTests
{
    [Fact]
    public void Vehicle_OnSave_Adds_CreatedEvent_When_Added()
    {
        // Arrange
        var role = new Role
        {
            Id = RoleEnum.Driver,
            Name = "Driver",
            Description = "A driver in the ride-sharing application."
        };
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            Email = "johndoe@example.com",
            Phone = "1234567890",
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
            Description = "A small car suitable for city driving."
        };

        user.UserRoles.Add(userRole);
        var vehicle = new Vehicle
        {
            Id = Guid.NewGuid(),
            User = user,
            UserId = user.Id,
            VehicleType = vehicleType,
            VehicleTypeId = VehicleTypeEnum.SmallCar,
            Vin = "1HGCM82633A123456",
            Plate = "ABC123",
            Make = "Toyota",
            Model = "Corolla",
            Year = 2020,
            RegisteredDate = DateTime.UtcNow
        };

        // Act
        vehicle.OnSave(EntityState.Added);

        // Assert
        Assert.Single(vehicle.DomainEvents);
        Assert.IsType<VehicleCreatedEvent>(vehicle.DomainEvents.First());
    }
}
