using AutoMapper;
using Moq;
using SmartRide.Application.Commands.Vehicles;
using SmartRide.Application.DTOs.Lookup;
using SmartRide.Application.DTOs.Vehicles;
using SmartRide.Application.Handlers.Vehicles;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Enums;
using SmartRide.Domain.Interfaces;

namespace SmartRide.UnitTests.Application.Handlers.Vehicles;

public class CreateVehicleCommandHandlerTests
{
    private readonly Mock<IRepository<Vehicle>> _mockVehicleRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly CreateVehicleCommandHandler _handler;

    public CreateVehicleCommandHandlerTests()
    {
        _mockVehicleRepository = new Mock<IRepository<Vehicle>>();
        _mockMapper = new Mock<IMapper>();
        _handler = new CreateVehicleCommandHandler(_mockVehicleRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_Should_Create_Vehicle_And_Return_Response()
    {
        // Arrange
        var command = new CreateVehicleCommand
        {
            UserId = Guid.NewGuid(),
            VehicleTypeId = VehicleTypeEnum.SmallCar,
            Vin = "1HGCM82633A123456",
            Plate = "ABC123",
            Make = "Toyota",
            Model = "Corolla",
            Year = 2020,
            RegisteredDate = DateTime.UtcNow.AddDays(-30)
        };
        var vehicle = new Vehicle
        {
            Id = Guid.NewGuid(),
            UserId = command.UserId,
            VehicleTypeId = command.VehicleTypeId,
            Vin = command.Vin,
            Plate = command.Plate,
            Make = command.Make,
            Model = command.Model,
            Year = command.Year,
            RegisteredDate = command.RegisteredDate
        };
        var response = new CreateVehicleResponseDTO
        {
            VehicleId = vehicle.Id,
            UserId = vehicle.UserId,
            Make = vehicle.Make,
            Model = vehicle.Model,
            Plate = vehicle.Plate,
            Vin = vehicle.Vin,
            Year = vehicle.Year,
            RegisteredDate = vehicle.RegisteredDate,
            VehicleType = new VehicleTypeDTO
            {
                VehicleTypeId = vehicle.VehicleTypeId,
                Name = vehicle.VehicleTypeId.ToString(),
                Capacity = 4
            }
        };

        _mockMapper.Setup(m => m.Map<Vehicle>(command)).Returns(vehicle);
        _mockVehicleRepository.Setup(r => r.CreateAsync(vehicle, It.IsAny<CancellationToken>())).ReturnsAsync(vehicle);
        _mockMapper.Setup(m => m.Map<CreateVehicleResponseDTO>(vehicle)).Returns(response);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(response.VehicleId, result.VehicleId);
        Assert.Equal(response.Make, result.Make);
        _mockVehicleRepository.Verify(r => r.CreateAsync(vehicle, It.IsAny<CancellationToken>()), Times.Once);
    }
}
