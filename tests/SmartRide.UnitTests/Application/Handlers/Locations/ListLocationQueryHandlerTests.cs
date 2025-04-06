using AutoMapper;
using Moq;
using SmartRide.Application.DTOs.Locations;
using SmartRide.Application.Handlers.Locations;
using SmartRide.Application.Queries.Locations;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;
using System.Linq.Expressions;

namespace SmartRide.UnitTests.Application.Handlers.Locations;

public class ListLocationQueryHandlerTests
{
    private readonly Mock<IRepository<Location>> _mockLocationRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly ListLocationQueryHandler _handler;

    public ListLocationQueryHandlerTests()
    {
        _mockLocationRepository = new Mock<IRepository<Location>>();
        _mockMapper = new Mock<IMapper>();
        _handler = new ListLocationQueryHandler(_mockLocationRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_Locations_With_Correct_Count_And_Ids()
    {
        // Arrange
        var locations = new List<Location>
        {
            new() { Id = Guid.NewGuid(), Address = "123 Main St", Latitude = 45.0, Longitude = -93.0 },
            new() { Id = Guid.NewGuid(), Address = "456 Elm St", Latitude = 40.0, Longitude = -75.0 }
        };
        var responseDtos = locations.Select(l => new ListLocationResponseDTO
        {
            LocationId = l.Id,
            Address = l.Address,
            Latitude = l.Latitude,
            Longitude = l.Longitude
        }).ToList();

        _mockLocationRepository.Setup(r => r.GetWithFilterAsync(
            It.IsAny<Expression<Func<Location, bool>>>(),
            It.IsAny<Expression<Func<Location, ListLocationResponseDTO>>>(),
            It.IsAny<Expression<Func<Location, object>>>(),
            It.IsAny<bool>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(locations);

        _mockMapper.Setup(m => m.Map<List<ListLocationResponseDTO>>(locations)).Returns(responseDtos);

        var query = new ListLocationQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(locations.First().Id, result.First().LocationId);
        Assert.Equal(locations.Last().Id, result.Last().LocationId);
    }

    [Fact]
    public async Task Handle_Should_Return_Empty_List_When_No_Locations_Found()
    {
        // Arrange
        _mockLocationRepository.Setup(r => r.GetWithFilterAsync(
            It.IsAny<Expression<Func<Location, bool>>>(),
            It.IsAny<Expression<Func<Location, ListLocationResponseDTO>>>(),
            It.IsAny<Expression<Func<Location, object>>>(),
            It.IsAny<bool>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(new List<Location>());

        var query = new ListLocationQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }
}
