using AutoMapper;
using Moq;
using SmartRide.Application.DTOs.Locations;
using SmartRide.Application.DTOs.Lookup;
using SmartRide.Application.DTOs.Rides;
using SmartRide.Application.DTOs.Users;
using SmartRide.Application.DTOs.Vehicles;
using SmartRide.Application.Handlers.Rides;
using SmartRide.Application.Queries.Rides;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Enums;
using SmartRide.Domain.Interfaces;

namespace SmartRide.UnitTests.Application.Handlers.Rides;

public class GetRideByIdQueryHandlerTests
{
    private readonly Mock<IRepository<Ride>> _mockRideRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetRideByIdQueryHandler _handler;

    public GetRideByIdQueryHandlerTests()
    {
        _mockRideRepository = new Mock<IRepository<Ride>>();
        _mockMapper = new Mock<IMapper>();
        _handler = new GetRideByIdQueryHandler(_mockRideRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_Ride_When_Ride_Is_Found()
    {
        // Arrange
        var rideId = Guid.NewGuid();
        var passengerRole = new RoleDTO
        {
            RoleId = RoleEnum.Passenger,
            Name = "Passenger"
        };
        var vehicleType = new VehicleTypeDTO
        {
            VehicleTypeId = VehicleTypeEnum.SmallCar,
            Name = "Small Car"
        };
        var driverRole = new RoleDTO
        {
            RoleId = RoleEnum.Driver,
            Name = "Driver"
        };
        var ride = new Ride
        {
            Id = rideId,
            PassengerId = Guid.NewGuid(),
            DriverId = Guid.NewGuid(),
            VehicleId = Guid.NewGuid(),
            VehicleTypeId = VehicleTypeEnum.Motorbike,
            Status = RideStatusEnum.Travelling,
            PickupLocationId = Guid.NewGuid(),
            DestinationId = Guid.NewGuid(),
            Fare = 100,
            Notes = "Test ride"
        };
        var driver = new GetUserResponseDTO
        {
            UserId = (Guid)ride.DriverId,
            FirstName = "Test Driver",
            Email = "driver@example.com",
            Phone = "+98 765 432 109",
            Roles = [driverRole]
        };
        var responseDto = new GetRideResponseDTO
        {
            RideId = rideId,
            Passenger = new GetUserResponseDTO
            {
                UserId = ride.PassengerId,
                FirstName = "Test Passenger",
                Email = "passenger@example.com",
                Phone = "+123 456 789 012",
                Roles = [passengerRole]
            },
            Driver = driver,
            Vehicle = new GetVehicleResponseDTO
            {
                VehicleId = (Guid)ride.VehicleId,
                User = driver,
                VehicleType = vehicleType,
                Vin = "1HGCM82633A123456",
                Plate = "ABC123",
                Make = "Toyota",
                Model = "Corolla",
                Year = 2020,
                RegisteredDate = DateTime.UtcNow.AddYears(-1),
            },
            VehicleType = vehicleType,
            RideType = RideTypeEnum.Private,
            RideStatus = RideStatusEnum.Travelling,
            PickupLocation = new GetLocationResponseDTO
            {
                LocationId = Guid.NewGuid(),
                Address = "123 Main St",
                Latitude = 45.0,
                Longitude = -93.0
            },
            Destination = new GetLocationResponseDTO
            {
                LocationId = Guid.NewGuid(),
                Address = "456 Elm St",
                Latitude = 45.5,
                Longitude = -93.5
            },
            Fare = ride.Fare,
            Notes = ride.Notes
        };

        _mockRideRepository.Setup(r => r.GetByIdAsync(rideId, It.IsAny<List<string>>(), It.IsAny<CancellationToken>())).ReturnsAsync(ride);
        _mockMapper.Setup(m => m.Map<GetRideResponseDTO>(ride)).Returns(responseDto);

        var query = new GetRideByIdQuery { RideId = rideId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(responseDto.RideId, result.RideId);
        Assert.Equal(responseDto.Fare, result.Fare);
        _mockRideRepository.Verify(r => r.GetByIdAsync(rideId, It.IsAny<List<string>>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_When_Ride_Is_Not_Found()
    {
        // Arrange
        var rideId = Guid.NewGuid();
        _mockRideRepository.Setup(r => r.GetByIdAsync(rideId, It.IsAny<List<string>>(), It.IsAny<CancellationToken>())).ReturnsAsync(null as Ride);

        var query = new GetRideByIdQuery { RideId = rideId };

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(query, CancellationToken.None));
    }
}
