using AutoMapper;
using Moq;
using SmartRide.Application.Commands.Rides;
using SmartRide.Application.DTOs.Rides;
using SmartRide.Application.Handlers.Rides;
using SmartRide.Common.Exceptions;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Enums;
using SmartRide.Domain.Interfaces;

namespace SmartRide.UnitTests.Application.Handlers.Rides;

public class UpdateRideCommandHandlerTests
{
    private readonly Mock<IRepository<Ride>> _mockRideRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly UpdateRideCommandHandler _handler;

    public UpdateRideCommandHandlerTests()
    {
        _mockRideRepository = new Mock<IRepository<Ride>>();
        _mockMapper = new Mock<IMapper>();
        _handler = new UpdateRideCommandHandler(_mockRideRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_Should_Update_Ride_And_Return_Response()
    {
        // Arrange
        var rideId = Guid.NewGuid();
        var command = new UpdateRideCommand
        {
            RideId = rideId,
            Fare = 150,
            Notes = "Updated notes"
        };
        var ride = new Ride
        {
            Id = rideId,
            PassengerId = Guid.NewGuid(),
            VehicleType = VehicleTypeEnum.SmallCar,
            RideType = RideTypeEnum.Private,
            PickupLocationId = Guid.NewGuid(),
            DestinationId = Guid.NewGuid(),
            Fare = 100,
            Notes = "Old notes"
        };
        var updatedRide = new Ride
        {
            Id = rideId,
            PassengerId = ride.PassengerId,
            VehicleType = ride.VehicleType,
            RideType = ride.RideType,
            PickupLocationId = ride.PickupLocationId,
            DestinationId = ride.DestinationId,
            Fare = command.Fare.Value,
            Notes = command.Notes
        };
        var response = new UpdateRideResponseDTO
        {
            RideId = rideId,
            Fare = command.Fare,
            Notes = command.Notes
        };

        _mockRideRepository.Setup(r => r.GetByIdAsync(rideId, It.IsAny<CancellationToken>())).ReturnsAsync(ride);
        _mockRideRepository.Setup(r => r.UpdateAsync(It.IsAny<Ride>(), It.IsAny<CancellationToken>())).ReturnsAsync(updatedRide);
        _mockMapper.Setup(m => m.Map<UpdateRideResponseDTO>(updatedRide)).Returns(response);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(response.RideId, result.RideId);
        Assert.Equal(response.Fare, result.Fare);
        _mockRideRepository.Verify(r => r.UpdateAsync(It.Is<Ride>(r => r.Id == command.RideId), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_When_Ride_Not_Found()
    {
        // Arrange
        var command = new UpdateRideCommand { RideId = Guid.NewGuid() };
        _mockRideRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(null as Ride);

        // Act & Assert
        await Assert.ThrowsAsync<BaseException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
