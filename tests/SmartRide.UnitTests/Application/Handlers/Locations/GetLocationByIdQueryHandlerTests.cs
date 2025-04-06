using AutoMapper;
using Moq;
using SmartRide.Application.DTOs.Locations;
using SmartRide.Application.Handlers.Locations;
using SmartRide.Application.Queries.Locations;
using SmartRide.Common.Exceptions;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.UnitTests.Application.Handlers.Locations;

public class GetLocationByIdQueryHandlerTests
{
  private readonly Mock<IRepository<Location>> _mockLocationRepository;
  private readonly Mock<IMapper> _mockMapper;
  private readonly GetLocationByIdQueryHandler _handler;

  public GetLocationByIdQueryHandlerTests()
  {
    _mockLocationRepository = new Mock<IRepository<Location>>();
    _mockMapper = new Mock<IMapper>();
    _handler = new GetLocationByIdQueryHandler(_mockLocationRepository.Object, _mockMapper.Object);
  }

  [Fact]
  public async Task Handle_Should_Return_Location_When_Location_Is_Found()
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
    var responseDto = new GetLocationResponseDTO
    {
      LocationId = locationId,
      Address = location.Address,
      Latitude = location.Latitude,
      Longitude = location.Longitude
    };

    _mockLocationRepository.Setup(r => r.GetByIdAsync(locationId, It.IsAny<CancellationToken>())).ReturnsAsync(location);
    _mockMapper.Setup(m => m.Map<GetLocationResponseDTO>(location)).Returns(responseDto);

    var query = new GetLocationByIdQuery { LocationId = locationId };

    // Act
    var result = await _handler.Handle(query, CancellationToken.None);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(responseDto.LocationId, result.LocationId);
    Assert.Equal(responseDto.Address, result.Address);
    _mockLocationRepository.Verify(r => r.GetByIdAsync(locationId, It.IsAny<CancellationToken>()), Times.Once);
  }

  [Fact]
  public async Task Handle_Should_Throw_Exception_When_Location_Is_Not_Found()
  {
    // Arrange
    var locationId = Guid.NewGuid();
    _mockLocationRepository.Setup(r => r.GetByIdAsync(locationId, It.IsAny<CancellationToken>())).ReturnsAsync(null as Location);

    var query = new GetLocationByIdQuery { LocationId = locationId };

    // Act & Assert
    await Assert.ThrowsAsync<BaseException>(() => _handler.Handle(query, CancellationToken.None));
  }
}
