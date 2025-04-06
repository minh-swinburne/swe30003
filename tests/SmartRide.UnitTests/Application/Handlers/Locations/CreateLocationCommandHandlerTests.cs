using AutoMapper;
using Moq;
using SmartRide.Application.Commands.Locations;
using SmartRide.Application.DTOs.Locations;
using SmartRide.Application.Handlers.Locations;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.UnitTests.Application.Handlers.Locations;

public class CreateLocationCommandHandlerTests
{
    private readonly Mock<IRepository<Location>> _mockLocationRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly CreateLocationCommandHandler _handler;

    public CreateLocationCommandHandlerTests()
    {
        _mockLocationRepository = new Mock<IRepository<Location>>();
        _mockMapper = new Mock<IMapper>();
        _handler = new CreateLocationCommandHandler(_mockLocationRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_Should_Create_Location_And_Return_Response()
    {
        // Arrange
        var command = new CreateLocationCommand
        {
            Address = "123 Main St",
            Latitude = 45.0,
            Longitude = -93.0,
            UserId = Guid.NewGuid()
        };
        var location = new Location
        {
            Id = Guid.NewGuid(),
            Address = command.Address,
            Latitude = command.Latitude,
            Longitude = command.Longitude,
            UserId = command.UserId
        };
        var response = new CreateLocationResponseDTO
        {
            LocationId = location.Id,
            Address = location.Address,
            Latitude = location.Latitude,
            Longitude = location.Longitude,
            UserId = location.UserId
        };

        _mockMapper.Setup(m => m.Map<Location>(command)).Returns(location);
        _mockLocationRepository.Setup(r => r.CreateAsync(location, It.IsAny<CancellationToken>())).ReturnsAsync(location);
        _mockMapper.Setup(m => m.Map<CreateLocationResponseDTO>(location)).Returns(response);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(response.LocationId, result.LocationId);
        Assert.Equal(response.Address, result.Address);
        _mockLocationRepository.Verify(r => r.CreateAsync(location, It.IsAny<CancellationToken>()), Times.Once);
    }
}
