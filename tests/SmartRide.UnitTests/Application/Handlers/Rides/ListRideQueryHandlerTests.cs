using AutoMapper;
using Moq;
using SmartRide.Application.DTOs.Rides;
using SmartRide.Application.Handlers.Rides;
using SmartRide.Application.Queries.Rides;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Enums;
using SmartRide.Domain.Interfaces;
using System.Linq.Expressions;

namespace SmartRide.UnitTests.Application.Handlers.Rides;

public class ListRideQueryHandlerTests
{
    private readonly Mock<IRepository<Ride>> _mockRideRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly ListRideQueryHandler _handler;

    public ListRideQueryHandlerTests()
    {
        _mockRideRepository = new Mock<IRepository<Ride>>();
        _mockMapper = new Mock<IMapper>();
        _handler = new ListRideQueryHandler(_mockRideRepository.Object, _mockMapper.Object);
    }

    private static List<Ride> CreateMockRides()
    {
        return
        [
            new()
            {
                Id = Guid.NewGuid(),
                PassengerId = Guid.NewGuid(),
                DriverId = Guid.NewGuid(),
                VehicleId = Guid.NewGuid(),
                VehicleTypeId = VehicleTypeEnum.SmallCar,
                RideType = RideTypeEnum.Standard,
                Status = RideStatusEnum.Completed,
                PickupLocationId = Guid.NewGuid(),
                DestinationId = Guid.NewGuid(),
                PickupETA = DateTime.UtcNow.AddMinutes(-15),
                PickupATA = DateTime.UtcNow.AddMinutes(-12),
                ArrivalETA = DateTime.UtcNow.AddMinutes(-2),
                ArrivalATA = DateTime.UtcNow,
                Fare = 100,
                Notes = "Ride 1"
            },
            new()
            {
                Id = Guid.NewGuid(),
                PassengerId = Guid.NewGuid(),
                DriverId = Guid.NewGuid(),
                VehicleId = Guid.NewGuid(),
                VehicleTypeId = VehicleTypeEnum.LargeCar,
                RideType = RideTypeEnum.Shared,
                Status = RideStatusEnum.Picking,
                PickupLocationId = Guid.NewGuid(),
                DestinationId = Guid.NewGuid(),
                PickupETA = DateTime.UtcNow.AddMinutes(5),
                ArrivalETA = DateTime.UtcNow.AddMinutes(30),
                Fare = 200,
                Notes = "Ride 2"
            }
        ];
    }

    private static List<ListRideResponseDTO> MapToResponseDTOs(List<Ride> rides)
    {
        return rides.Select(r => new ListRideResponseDTO
        {
            RideId = r.Id,
            PassengerId = r.PassengerId,
            VehicleType = r.VehicleTypeId,
            DriverId = r.DriverId,
            VehicleId = r.VehicleId,
            RideType = r.RideType,
            RideStatus = r.Status,
            PickupLocationId = r.PickupLocationId,
            DestinationId = r.DestinationId,
            PickupETA = r.PickupETA,
            PickupATA = r.PickupATA,
            ArrivalETA = r.ArrivalETA,
            ArrivalATA = r.ArrivalATA
        }).ToList();
    }

    private void SetupMocks(List<Ride> rides, List<ListRideResponseDTO> expected)
    {
        _mockRideRepository.Setup(x => x.GetWithFilterAsync(
            It.IsAny<Expression<Func<Ride, bool>>>(),
            It.IsAny<Expression<Func<Ride, ListRideResponseDTO>>>(),
            It.IsAny<Expression<Func<Ride, object>>>(),
            It.IsAny<bool>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<List<Expression<Func<Ride, object>>>>(),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(rides);

        _mockMapper.Setup(x => x.Map<List<ListRideResponseDTO>>(rides)).Returns(expected);
    }

    [Fact]
    public async Task Handle_Should_Return_Rides_With_Correct_Count_And_Ids()
    {
        // Arrange
        var query = new ListRideQuery();
        var rides = CreateMockRides();
        var expected = MapToResponseDTOs(rides);
        SetupMocks(rides, expected);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(rides.First().Id, result.First().RideId);
        Assert.Equal(rides.Last().Id, result.Last().RideId);
    }

    [Fact]
    public async Task Handle_Should_Return_Empty_List_When_No_Rides_Found()
    {
        // Arrange
        var query = new ListRideQuery();
        SetupMocks(new List<Ride>(), new List<ListRideResponseDTO>());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }
}
