using AutoMapper;
using Moq;
using SmartRide.Application.Commands.Rides;
using SmartRide.Application.DTOs.Rides;
using SmartRide.Application.Handlers.Rides;
using SmartRide.Common.Exceptions;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Entities.Join;
using SmartRide.Domain.Enums;
using SmartRide.Domain.Interfaces;

namespace SmartRide.UnitTests.Application.Handlers.Rides;

public class MatchRideCommandHandlerTests
{
    private readonly Mock<IRepository<Ride>> _mockRideRepository;
    private readonly Mock<IRepository<User>> _mockUserRepository;
    private readonly Mock<IRepository<Vehicle>> _mockVehicleRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly MatchRideCommandHandler _handler;

    public MatchRideCommandHandlerTests()
    {
        _mockRideRepository = new Mock<IRepository<Ride>>();
        _mockUserRepository = new Mock<IRepository<User>>();
        _mockVehicleRepository = new Mock<IRepository<Vehicle>>();
        _mockMapper = new Mock<IMapper>();
        _handler = new MatchRideCommandHandler(_mockRideRepository.Object, _mockUserRepository.Object, _mockVehicleRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_Should_Match_Ride_And_Return_Response()
    {
        // Arrange
        var rideId = Guid.NewGuid();
        var driverId = Guid.NewGuid();
        var vehicleId = Guid.NewGuid();

        var command = new MatchRideCommand
        {
            RideId = rideId,
            DriverId = driverId,
            VehicleId = vehicleId,
            PickupETA = DateTime.UtcNow.AddMinutes(10)
        };

        var driver = new User
        {
            Id = driverId,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Phone = "1234567890",
            UserRoles =
            [
                new() { UserId = driverId, RoleId = RoleEnum.Driver }
            ]
        };

        var ride = new Ride
        {
            Id = rideId,
            PassengerId = Guid.NewGuid(),
            VehicleType = VehicleTypeEnum.SmallCar,
            Status = RideStatusEnum.Pending,
            PickupLocationId = Guid.NewGuid(),
            DestinationId = Guid.NewGuid(),
        };

        var vehicle = new Vehicle
        {
            Id = vehicleId,
            UserId = driverId,
            VehicleTypeId = VehicleTypeEnum.SmallCar,
            Vin = "1HGCM82633A123456",
            Plate = "ABC123",
            Make = "Toyota",
            Model = "Corolla",
            Year = 2020,
            RegisteredDate = DateTime.UtcNow.AddDays(-30)
        };

        var updatedRide = new Ride
        {
            Id = rideId,
            PassengerId = ride.PassengerId,
            DriverId = driverId,
            VehicleId = vehicleId,
            VehicleType = ride.VehicleType,
            Status = RideStatusEnum.Picking,
            PickupLocationId = ride.PickupLocationId,
            DestinationId = ride.DestinationId,
            PickupETA = command.PickupETA,
        };

        var response = new MatchRideResponseDTO
        {
            RideId = rideId,
            DriverId = driverId,
            VehicleId = vehicleId,
            RideStatus = updatedRide.Status,
            PickupETA = command.PickupETA,
        };

        _mockRideRepository.Setup(r => r.GetByIdAsync(rideId, It.IsAny<CancellationToken>())).ReturnsAsync(ride);
        _mockUserRepository.Setup(u => u.GetByIdAsync(driverId, It.IsAny<CancellationToken>())).ReturnsAsync(driver);
        _mockVehicleRepository.Setup(v => v.GetByIdAsync(vehicleId, It.IsAny<CancellationToken>())).ReturnsAsync(vehicle);
        _mockRideRepository.Setup(r => r.UpdateAsync(It.IsAny<Ride>(), It.IsAny<CancellationToken>())).ReturnsAsync(updatedRide);
        _mockMapper.Setup(m => m.Map<MatchRideResponseDTO>(updatedRide)).Returns(response);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(response.RideId, result.RideId);
        Assert.Equal(response.DriverId, result.DriverId);
        Assert.Equal(response.VehicleId, result.VehicleId);
        Assert.Equal(response.RideStatus, result.RideStatus);
        _mockRideRepository.Verify(r => r.UpdateAsync(It.Is<Ride>(r => r.Id == command.RideId), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_When_Ride_Not_Found()
    {
        // Arrange
        var command = new MatchRideCommand { RideId = Guid.NewGuid() };
        _mockRideRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(null as Ride);

        // Act & Assert
        await Assert.ThrowsAsync<BaseException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_When_Vehicle_Not_Found()
    {
        // Arrange
        var rideId = Guid.NewGuid();
        var command = new MatchRideCommand { RideId = rideId, VehicleId = Guid.NewGuid() };

        var ride = new Ride
        {
            Id = rideId,
            PassengerId = Guid.NewGuid(),
            VehicleType = VehicleTypeEnum.SmallCar,
            Status = RideStatusEnum.Pending,
            PickupLocationId = Guid.NewGuid(),
            DestinationId = Guid.NewGuid(),
        };

        _mockRideRepository.Setup(r => r.GetByIdAsync(rideId, It.IsAny<CancellationToken>())).ReturnsAsync(ride);
        _mockVehicleRepository.Setup(v => v.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(null as Vehicle);

        // Act & Assert
        await Assert.ThrowsAsync<BaseException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
