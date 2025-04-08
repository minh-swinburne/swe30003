using AutoMapper;
using Moq;
using SmartRide.Application.DTOs.Lookup;
using SmartRide.Application.DTOs.Users;
using SmartRide.Application.DTOs.Vehicles;
using SmartRide.Application.Handlers.Vehicles;
using SmartRide.Application.Queries.Vehicles;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Enums;
using SmartRide.Domain.Interfaces;

namespace SmartRide.UnitTests.Application.Handlers.Vehicles;

public class GetVehicleByIdQueryHandlerTests
{
    private readonly Mock<IRepository<Vehicle>> _mockVehicleRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetVehicleByIdQueryHandler _handler;

    public GetVehicleByIdQueryHandlerTests()
    {
        _mockVehicleRepository = new Mock<IRepository<Vehicle>>();
        _mockMapper = new Mock<IMapper>();
        _handler = new GetVehicleByIdQueryHandler(_mockVehicleRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_Vehicle_When_Vehicle_Is_Found()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var vehicleId = Guid.NewGuid();
        var vehicle = new Vehicle
        {
            Id = vehicleId,
            UserId = userId,
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
            UserId = userId,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Phone = "1234567890",
            Roles = [new() {
                RoleId = RoleEnum.Driver,
                Name = "User"
            }]
        };
        var responseDto = new GetVehicleResponseDTO
        {
            VehicleId = vehicleId,
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

        _mockVehicleRepository.Setup(r => r.GetByIdAsync(vehicleId, It.IsAny<List<string>>(), It.IsAny<CancellationToken>())).ReturnsAsync(vehicle);
        _mockMapper.Setup(m => m.Map<GetVehicleResponseDTO>(vehicle)).Returns(responseDto);

        var query = new GetVehicleByIdQuery { VehicleId = vehicleId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(responseDto.VehicleId, result.VehicleId);
        Assert.Equal(responseDto.Make, result.Make);
        _mockVehicleRepository.Verify(r => r.GetByIdAsync(vehicleId, It.IsAny<List<string>>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Return_Null_When_Vehicle_Is_Not_Found()
    {
        // Arrange
        var vehicleId = Guid.NewGuid();
        _mockVehicleRepository.Setup(r => r.GetByIdAsync(vehicleId, It.IsAny<List<string>>(), It.IsAny<CancellationToken>())).ReturnsAsync(null as Vehicle);

        var query = new GetVehicleByIdQuery { VehicleId = vehicleId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Null(result);
        _mockVehicleRepository.Verify(r => r.GetByIdAsync(vehicleId, It.IsAny<List<string>>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
