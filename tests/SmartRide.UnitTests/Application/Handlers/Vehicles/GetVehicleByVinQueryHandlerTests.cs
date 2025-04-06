using AutoMapper;
using MockQueryable;
using Moq;
using SmartRide.Application.DTOs.Lookup;
using SmartRide.Application.DTOs.Vehicles;
using SmartRide.Application.Handlers.Vehicles;
using SmartRide.Application.Queries.Vehicles;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Enums;
using SmartRide.Domain.Interfaces;

namespace SmartRide.UnitTests.Application.Handlers.Vehicles;

public class GetVehicleByVinQueryHandlerTests
{
    private readonly Mock<IRepository<Vehicle>> _mockVehicleRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetVehicleByVinQueryHandler _handler;

    public GetVehicleByVinQueryHandlerTests()
    {
        _mockVehicleRepository = new Mock<IRepository<Vehicle>>();
        _mockMapper = new Mock<IMapper>();
        _handler = new GetVehicleByVinQueryHandler(_mockVehicleRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_Vehicle_When_Vin_Is_Found()
    {
        // Arrange
        var vin = "1HGCM82633A123456";
        var userId = Guid.NewGuid();
        var vehicleId = Guid.NewGuid();
        var vehicle = new Vehicle
        {
            Id = vehicleId,
            UserId = userId,
            VehicleTypeId = VehicleTypeEnum.SmallCar,
            Make = "Toyota",
            Model = "Corolla",
            Plate = "ABC123",
            Vin = vin,
            Year = 2020,
            RegisteredDate = DateTime.UtcNow.AddYears(-1),
        };
        var responseDto = new GetVehicleResponseDTO
        {
            VehicleId = vehicleId,
            UserId = userId,
            Make = vehicle.Make,
            Model = vehicle.Model,
            Plate = vehicle.Plate,
            Vin = vehicle.Vin,
            Year = vehicle.Year,
            RegisteredDate = vehicle.RegisteredDate,
            VehicleType = new VehicleTypeDTO
            {
                VehicleTypeId = (byte)vehicle.VehicleTypeId,
                Name = vehicle.VehicleTypeId.ToString()
            },
        };

        _mockVehicleRepository.Setup(r => r.Query(It.IsAny<CancellationToken>()))
            .Returns(new List<Vehicle> { vehicle }.BuildMock());
        _mockMapper.Setup(m => m.Map<GetVehicleResponseDTO>(vehicle)).Returns(responseDto);

        var query = new GetVehicleByVinQuery { Vin = vin };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(responseDto.Vin, result.Vin);
        _mockVehicleRepository.Verify(r => r.Query(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Return_Null_When_Vin_Is_Not_Found()
    {
        // Arrange
        var vin = "INVALIDVIN@1234567";
        _mockVehicleRepository.Setup(r => r.Query(It.IsAny<CancellationToken>()))
            .Returns(new List<Vehicle>().BuildMock());

        var query = new GetVehicleByVinQuery { Vin = vin };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Null(result);
        _mockVehicleRepository.Verify(r => r.Query(It.IsAny<CancellationToken>()), Times.Once);
    }
}
