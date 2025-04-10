using AutoMapper;
using Moq;
using SmartRide.Application.Commands.Rides;
using SmartRide.Application.DTOs.Rides;
using SmartRide.Application.Handlers.Rides;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Enums;
using SmartRide.Domain.Interfaces;
using System.Linq.Expressions;

namespace SmartRide.UnitTests.Application.Handlers.Rides;

public class CreateRideCommandHandlerTests
{
    private readonly Mock<IRepository<Ride>> _mockRideRepository;
    private readonly Mock<IRepository<User>> _mockUserRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly CreateRideCommandHandler _handler;

    public CreateRideCommandHandlerTests()
    {
        _mockRideRepository = new Mock<IRepository<Ride>>();
        _mockUserRepository = new Mock<IRepository<User>>();
        _mockMapper = new Mock<IMapper>();
        _handler = new CreateRideCommandHandler(_mockRideRepository.Object, _mockUserRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_Should_Create_Ride_And_Return_Response()
    {
        // Arrange
        var command = new CreateRideCommand
        {
            PassengerId = Guid.NewGuid(),
            VehicleType = VehicleTypeEnum.SmallCar,
            PickupLocationId = Guid.NewGuid(),
            DestinationId = Guid.NewGuid(),
            Fare = 100,
            Notes = "Test ride"
        };
        var user = new User
        {
            Id = command.PassengerId,
            FirstName = "John",
            LastName = "Doe",
            Email = "johndoe@example.com",
            Phone = "0123456789"
        };
        var ride = new Ride
        {
            Id = Guid.NewGuid(),
            PassengerId = command.PassengerId,
            VehicleTypeId = VehicleTypeEnum.SmallCar,
            PickupLocationId = Guid.NewGuid(),
            DestinationId = Guid.NewGuid(),
            Fare = command.Fare,
            Notes = command.Notes
        };
        var response = new CreateRideResponseDTO
        {
            RideId = ride.Id,
            PassengerId = ride.PassengerId,
            VehicleType = ride.VehicleTypeId,
            RideType = ride.RideType,
            RideStatus = RideStatusEnum.Pending,
            PickupLocationId = ride.PickupLocationId,
            DestinationId = ride.DestinationId,
            Fare = ride.Fare,
            Notes = ride.Notes
        };

        _mockMapper.Setup(m => m.Map<Ride>(command)).Returns(ride);
        _mockRideRepository.Setup(r => r.CreateAsync(ride, It.IsAny<CancellationToken>())).ReturnsAsync(ride);
        _mockUserRepository.Setup(r => r.GetByIdAsync(command.PassengerId, It.IsAny<List<Expression<Func<User, object>>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _mockMapper.Setup(m => m.Map<CreateRideResponseDTO>(ride)).Returns(response);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(response.RideId, result.RideId);
        Assert.Equal(response.Fare, result.Fare);
        _mockRideRepository.Verify(r => r.CreateAsync(ride, It.IsAny<CancellationToken>()), Times.Once);
    }
}
