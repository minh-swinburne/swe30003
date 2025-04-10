using AutoMapper;
using Moq;
using SmartRide.Application.Commands.Locations;
using SmartRide.Application.DTOs.Locations;
using SmartRide.Application.Handlers.Locations;
using SmartRide.Common.Exceptions;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;
using System.Linq.Expressions;

namespace SmartRide.UnitTests.Application.Handlers.Locations;

public class UpdateLocationCommandHandlerTests
{
    private readonly Mock<IRepository<Location>> _mockLocationRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly UpdateLocationCommandHandler _handler;

    public UpdateLocationCommandHandlerTests()
    {
        _mockLocationRepository = new Mock<IRepository<Location>>();
        _mockMapper = new Mock<IMapper>();
        _handler = new UpdateLocationCommandHandler(_mockLocationRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_Should_Update_Location_And_Return_Response()
    {
        // Arrange
        var locationId = Guid.NewGuid();
        var command = new UpdateLocationCommand
        {
            LocationId = locationId,
            Address = "Updated Address",
            Latitude = 40.0,
            Longitude = -75.0
        };
        var location = new Location
        {
            Id = locationId,
            Address = "Old Address",
            Latitude = 45.0,
            Longitude = -93.0
        };
        var updatedLocation = new Location
        {
            Id = locationId,
            Address = command.Address,
            Latitude = command.Latitude,
            Longitude = command.Longitude
        };
        var response = new UpdateLocationResponseDTO
        {
            LocationId = locationId,
            Address = command.Address,
            Latitude = command.Latitude,
            Longitude = command.Longitude
        };

        _mockLocationRepository.Setup(r => r.GetByIdAsync(locationId, It.IsAny<List<Expression<Func<Location, object>>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(location);
        _mockLocationRepository.Setup(r => r.UpdateAsync(It.IsAny<Location>(), It.IsAny<CancellationToken>())).ReturnsAsync(updatedLocation);
        _mockMapper.Setup(m => m.Map<UpdateLocationResponseDTO>(updatedLocation)).Returns(response);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(response.LocationId, result.LocationId);
        Assert.Equal(response.Address, result.Address);
        _mockLocationRepository.Verify(r => r.UpdateAsync(It.Is<Location>(l => l.Id == command.LocationId), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_When_Location_Not_Found()
    {
        // Arrange
        var command = new UpdateLocationCommand { LocationId = Guid.NewGuid() };
        _mockLocationRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<List<Expression<Func<Location, object>>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(null as Location);

        // Act & Assert
        await Assert.ThrowsAsync<BaseException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
