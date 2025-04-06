using AutoMapper;
using Moq;
using SmartRide.Application.Commands.Vehicles;
using SmartRide.Application.DTOs.Vehicles;
using SmartRide.Application.Handlers.Vehicles;
using SmartRide.Common.Exceptions;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Enums;
using SmartRide.Domain.Interfaces;

namespace SmartRide.UnitTests.Application.Handlers.Vehicles;

public class UpdateVehicleCommandHandlerTests
{
    private readonly Mock<IRepository<Vehicle>> _mockVehicleRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly UpdateVehicleCommandHandler _handler;

    public UpdateVehicleCommandHandlerTests()
    {
        _mockVehicleRepository = new Mock<IRepository<Vehicle>>();
        _mockMapper = new Mock<IMapper>();
        _handler = new UpdateVehicleCommandHandler(_mockVehicleRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_Should_Update_Vehicle_And_Return_Response()
    {
        // Arrange
        var vehicleId = Guid.NewGuid();
        var command = new UpdateVehicleCommand
        {
            VehicleId = vehicleId,
            Make = "UpdatedMake",
            Model = "UpdatedModel"
        };
        var vehicle = new Vehicle
        {
            Id = vehicleId,
            UserId = Guid.NewGuid(),
            VehicleTypeId = VehicleTypeEnum.SmallCar,
            Vin = "1HGCM82633A123456",
            Plate = "ABC123",
            Make = "OldMake",
            Model = "OldModel",
            Year = 2020,
            RegisteredDate = DateTime.UtcNow.AddYears(-1),
        };
        var updatedVehicle = new Vehicle
        {
            Id = vehicleId,
            UserId = vehicle.UserId,
            VehicleTypeId = vehicle.VehicleTypeId,
            Vin = vehicle.Vin,
            Plate = vehicle.Plate,
            Make = command.Make,
            Model = command.Model,
            Year = vehicle.Year,
            RegisteredDate = vehicle.RegisteredDate
        };
        var response = new UpdateVehicleResponseDTO
        {
            VehicleId = vehicleId,
            Plate = vehicle.Plate,
            Make = command.Make,
            Model = command.Model,
            Year = vehicle.Year,
            RegisteredDate = vehicle.RegisteredDate,
        };

        _mockVehicleRepository.Setup(r => r.GetByIdAsync(vehicleId, It.IsAny<CancellationToken>())).ReturnsAsync(vehicle);
        _mockVehicleRepository.Setup(r => r.UpdateAsync(It.IsAny<Vehicle>(), It.IsAny<CancellationToken>())).ReturnsAsync(updatedVehicle);
        _mockMapper.Setup(m => m.Map<UpdateVehicleResponseDTO>(updatedVehicle)).Returns(response);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(response.VehicleId, result.VehicleId);
        Assert.Equal(response.Make, result.Make);
        _mockVehicleRepository.Verify(r => r.UpdateAsync(It.Is<Vehicle>(v => v.Id == command.VehicleId), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_When_Vehicle_Not_Found()
    {
        // Arrange
        var command = new UpdateVehicleCommand { VehicleId = Guid.NewGuid() };
        _mockVehicleRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(null as Vehicle);

        // Act & Assert
        await Assert.ThrowsAsync<BaseException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
