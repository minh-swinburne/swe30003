using AutoMapper;
using MockQueryable;
using Moq;
using SmartRide.Application.DTOs.Lookup;
using SmartRide.Application.DTOs.Users;
using SmartRide.Application.DTOs.Vehicles;
using SmartRide.Application.Handlers.Vehicles;
using SmartRide.Application.Queries.Vehicles;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Enums;
using SmartRide.Domain.Interfaces;
using System.Linq.Expressions;

namespace SmartRide.UnitTests.Application.Handlers.Vehicles;

public class GetVehicleByPlateQueryHandlerTests
{
    private readonly Mock<IRepository<Vehicle>> _mockVehicleRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetVehicleByPlateQueryHandler _handler;

    public GetVehicleByPlateQueryHandlerTests()
    {
        _mockVehicleRepository = new Mock<IRepository<Vehicle>>();
        _mockMapper = new Mock<IMapper>();
        _handler = new GetVehicleByPlateQueryHandler(_mockVehicleRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_Vehicle_When_Plate_Is_Found()
    {
        // Arrange
        var plate = "ABC123";
        var vehicle = new Vehicle
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            VehicleTypeId = VehicleTypeEnum.SmallCar,
            Vin = "1HGCM82633A123456",
            Plate = "ABC123",
            Make = "Toyota",
            Model = "Corolla",
            Year = 2020,
            RegisteredDate = DateTime.UtcNow.AddYears(-1),
        };
        var user = new GetUserResponseDTO
        {
            UserId = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Phone = "1234567890",
            Roles = [ new() {
                RoleId = RoleEnum.Driver,
                Name = "User"
            }]
        };
        var responseDto = new GetVehicleResponseDTO
        {
            VehicleId = vehicle.Id,
            User = user,
            Vin = vehicle.Vin,
            Plate = vehicle.Plate,
            Make = vehicle.Make,
            Model = vehicle.Model,
            Year = vehicle.Year,
            RegisteredDate = vehicle.RegisteredDate,
            VehicleType = new VehicleTypeDTO
            {
                VehicleTypeId = vehicle.VehicleTypeId,
                Name = vehicle.VehicleTypeId.ToString()
            },
        };

        _mockVehicleRepository.Setup(r => r.Query(It.IsAny<Expression<Func<Vehicle, bool>>>(), It.IsAny<IEnumerable<Expression<Func<Vehicle, object>>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(vehicle);
        _mockMapper.Setup(m => m.Map<GetVehicleResponseDTO>(vehicle)).Returns(responseDto);

        var query = new GetVehicleByPlateQuery { Plate = plate };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(responseDto.Plate, result.Plate);
        _mockVehicleRepository.Verify(r => r.Query(It.IsAny<Expression<Func<Vehicle, bool>>>(), It.IsAny<IEnumerable<Expression<Func<Vehicle, object>>>>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Return_Null_When_Plate_Is_Not_Found()
    {
        // Arrange
        var plate = "INVALID123";
        _mockVehicleRepository.Setup(r => r.Query(It.IsAny<Expression<Func<Vehicle, bool>>>(), It.IsAny<IEnumerable<Expression<Func<Vehicle, object>>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Vehicle)null!);

        var query = new GetVehicleByPlateQuery { Plate = plate };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Null(result);
        _mockVehicleRepository.Verify(r => r.Query(It.IsAny<Expression<Func<Vehicle, bool>>>(), It.IsAny<IEnumerable<Expression<Func<Vehicle, object>>>>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
