using Moq;
using SmartRide.Application.Commands.Rides;
using SmartRide.Application.DTOs.Rides;
using SmartRide.Application.Handlers.Rides;
using SmartRide.Common.Exceptions;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Enums;
using SmartRide.Domain.Interfaces;

namespace SmartRide.UnitTests.Application.Handlers.Rides;

public class DeleteRideCommandHandlerTests
{
    private readonly Mock<IRepository<Ride>> _mockRideRepository;
    private readonly DeleteRideCommandHandler _handler;

    public DeleteRideCommandHandlerTests()
    {
        _mockRideRepository = new Mock<IRepository<Ride>>();
        _handler = new DeleteRideCommandHandler(_mockRideRepository.Object);
    }

    [Fact]
    public async Task Handle_Should_Delete_Ride_And_Return_Response()
    {
        // Arrange
        var rideId = Guid.NewGuid();
        var ride = new Ride
        {
            Id = rideId,
            PassengerId = Guid.NewGuid(),
            VehicleType = VehicleTypeEnum.SmallCar,
            PickupLocationId = Guid.NewGuid(),
            DestinationId = Guid.NewGuid(),
            Fare = 100,
            Notes = "Test ride"
        };
        _mockRideRepository.Setup(r => r.GetByIdAsync(rideId, It.IsAny<CancellationToken>())).ReturnsAsync(ride);
        _mockRideRepository.Setup(r => r.DeleteAsync(rideId, It.IsAny<CancellationToken>())).ReturnsAsync(ride);

        var command = new DeleteRideCommand { RideId = rideId };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.Equal(rideId, result.RideId);
        _mockRideRepository.Verify(r => r.DeleteAsync(rideId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_When_Ride_Not_Found()
    {
        // Arrange
        var command = new DeleteRideCommand { RideId = Guid.NewGuid() };
        _mockRideRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(null as Ride);

        // Act & Assert
        await Assert.ThrowsAsync<BaseException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
