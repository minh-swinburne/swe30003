using Moq;
using SmartRide.Application.Commands.Vehicles;
using SmartRide.Application.DTOs.Vehicles;
using SmartRide.Application.Handlers.Vehicles;
using SmartRide.Common.Exceptions;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Enums;
using SmartRide.Domain.Interfaces;

namespace SmartRide.UnitTests.Application.Handlers.Vehicles;

public class DeleteVehicleCommandHandlerTests
{
    private readonly Mock<IRepository<Vehicle>> _mockVehicleRepository;
    private readonly DeleteVehicleCommandHandler _handler;

    public DeleteVehicleCommandHandlerTests()
    {
        _mockVehicleRepository = new Mock<IRepository<Vehicle>>();
        _handler = new DeleteVehicleCommandHandler(_mockVehicleRepository.Object);
    }

    [Fact]
    public async Task Handle_Should_Delete_Vehicle_And_Return_Response()
    {
        // Arrange
        var vehicleId = Guid.NewGuid();
        var vehicle = new Vehicle
        {
            Id = vehicleId,
            UserId = Guid.NewGuid(),
            VehicleTypeId = VehicleTypeEnum.SmallCar,
            Vin = "1HGCM82633A123456",
            Plate = "ABC123",
            Make = "Toyota",
            Model = "Corolla",
            Year = 2020,
            RegisteredDate = DateTime.UtcNow.AddYears(-1),
        };
        _mockVehicleRepository.Setup(r => r.GetByIdAsync(vehicleId, It.IsAny<CancellationToken>())).ReturnsAsync(vehicle);
        _mockVehicleRepository.Setup(r => r.DeleteAsync(vehicleId, It.IsAny<CancellationToken>())).ReturnsAsync(vehicle);

        var command = new DeleteVehicleCommand { VehicleId = vehicleId };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.Equal(vehicleId, result.VehicleId);
        _mockVehicleRepository.Verify(r => r.DeleteAsync(vehicleId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_When_Vehicle_Not_Found()
    {
        // Arrange
        var command = new DeleteVehicleCommand { VehicleId = Guid.NewGuid() };
        _mockVehicleRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(null as Vehicle);

        // Act & Assert
        await Assert.ThrowsAsync<BaseException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
