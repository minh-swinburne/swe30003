using AutoMapper;
using Moq;
using SmartRide.Application.DTOs.Rides;
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
        var ride = new Ride
        {
            Id = rideId,
            PassengerId = Guid.NewGuid(),
            DriverId = Guid.NewGuid(),
            VehicleId = Guid.NewGuid(),
            VehicleType = VehicleTypeEnum.Motorbike,
            Status = RideStatusEnum.Travelling,
            PickupLocationId = Guid.NewGuid(),
            DestinationId = Guid.NewGuid(),
            Fare = 100,
            Notes = "Test ride"
        };
        var responseDto = new GetRideResponseDTO
        {
            RideId = rideId,
            PassengerId = ride.PassengerId,
            DriverId = ride.DriverId,
            VehicleId = ride.VehicleId,
            VehicleType = ride.VehicleType,
            RideType = RideTypeEnum.Private,
            RideStatus = RideStatusEnum.Travelling,
            PickupLocationId = ride.PickupLocationId,
            DestinationId = ride.DestinationId,
            Fare = ride.Fare,
            Notes = ride.Notes
        };

        _mockRideRepository.Setup(r => r.GetByIdAsync(rideId, It.IsAny<CancellationToken>())).ReturnsAsync(ride);
        _mockMapper.Setup(m => m.Map<GetRideResponseDTO>(ride)).Returns(responseDto);

        var query = new GetRideByIdQuery { RideId = rideId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(responseDto.RideId, result.RideId);
        Assert.Equal(responseDto.Fare, result.Fare);
        _mockRideRepository.Verify(r => r.GetByIdAsync(rideId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_When_Ride_Is_Not_Found()
    {
        // Arrange
        var rideId = Guid.NewGuid();
        _mockRideRepository.Setup(r => r.GetByIdAsync(rideId, It.IsAny<CancellationToken>())).ReturnsAsync(null as Ride);

        var query = new GetRideByIdQuery { RideId = rideId };

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(query, CancellationToken.None));
    }
}
