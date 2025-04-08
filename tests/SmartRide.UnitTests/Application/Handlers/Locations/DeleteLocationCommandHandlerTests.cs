using Moq;
using SmartRide.Application.Commands.Locations;
using SmartRide.Application.DTOs.Locations;
using SmartRide.Application.Handlers.Locations;
using SmartRide.Common.Exceptions;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.UnitTests.Application.Handlers.Locations;

public class DeleteLocationCommandHandlerTests
{
    private readonly Mock<IRepository<Location>> _mockLocationRepository;
    private readonly DeleteLocationCommandHandler _handler;

    public DeleteLocationCommandHandlerTests()
    {
        _mockLocationRepository = new Mock<IRepository<Location>>();
        _handler = new DeleteLocationCommandHandler(_mockLocationRepository.Object);
    }

    [Fact]
    public async Task Handle_Should_Delete_Location_And_Return_Response()
    {
        // Arrange
        var locationId = Guid.NewGuid();
        var location = new Location
        {
            Id = locationId,
            Address = "123 Main St",
            Latitude = 45.0,
            Longitude = -93.0
        };
        _mockLocationRepository.Setup(r => r.GetByIdAsync(locationId, It.IsAny<List<string>>(), It.IsAny<CancellationToken>())).ReturnsAsync(location);
        _mockLocationRepository.Setup(r => r.DeleteAsync(locationId, It.IsAny<CancellationToken>())).ReturnsAsync(location);

        var command = new DeleteLocationCommand { LocationId = locationId };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.Equal(locationId, result.LocationId);
        _mockLocationRepository.Verify(r => r.DeleteAsync(locationId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_When_Location_Not_Found()
    {
        // Arrange
        var command = new DeleteLocationCommand { LocationId = Guid.NewGuid() };
        _mockLocationRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<List<string>>(), It.IsAny<CancellationToken>())).ReturnsAsync(null as Location);

        // Act & Assert
        await Assert.ThrowsAsync<BaseException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
