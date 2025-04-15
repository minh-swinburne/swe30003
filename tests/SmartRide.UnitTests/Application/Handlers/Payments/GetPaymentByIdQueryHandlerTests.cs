using AutoMapper;
using Moq;
using SmartRide.Application.DTOs.Locations;
using SmartRide.Application.DTOs.Lookup;
using SmartRide.Application.DTOs.Payments;
using SmartRide.Application.DTOs.Rides;
using SmartRide.Application.DTOs.Users;
using SmartRide.Application.Handlers.Payments;
using SmartRide.Application.Queries.Payments;
using SmartRide.Common.Exceptions;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Enums;
using SmartRide.Domain.Interfaces;
using System.Linq.Expressions;

namespace SmartRide.UnitTests.Application.Handlers.Payments;

public class GetPaymentByIdQueryHandlerTests
{
    private readonly Mock<IRepository<Payment>> _mockPaymentRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetPaymentByIdQueryHandler _handler;

    public GetPaymentByIdQueryHandlerTests()
    {
        _mockPaymentRepository = new Mock<IRepository<Payment>>();
        _mockMapper = new Mock<IMapper>();
        _handler = new GetPaymentByIdQueryHandler(_mockPaymentRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_Payment_When_Payment_Is_Found()
    {
        // Arrange
        var paymentId = Guid.NewGuid();
        var payment = new Payment
        {
            Id = paymentId,
            RideId = Guid.NewGuid(),
            Amount = 100
        };
        var paymentMethod = new PaymentMethodDTO
        {
            PaymentMethodId = PaymentMethodEnum.Cash,
            Name = "Cash"
        };
        var passengerRole = new RoleDTO
        {
            RoleId = RoleEnum.Passenger,
            Name = "Passenger"
        };
        var vehicleType = new VehicleTypeDTO
        {
            VehicleTypeId = VehicleTypeEnum.SmallCar,
            Name = "Small Car"
        };
        var driverRole = new RoleDTO
        {
            RoleId = RoleEnum.Driver,
            Name = "Driver"
        };
        GetPaymentResponseDTO responseDto = null!;
        responseDto = new GetPaymentResponseDTO
        {
            PaymentId = paymentId,
            Amount = payment.Amount,
            Currency = PaymentCurrencyEnum.USD.ToString(),
            Status = payment.Status,
            PaymentMethod = new PaymentMethodDTO
            {
                PaymentMethodId = PaymentMethodEnum.Cash,
                Name = "Cash"
            },
            Ride = new GetRideResponseDTO
            {
                RideId = payment.RideId,
                Payment = responseDto,
                VehicleType = vehicleType,
                RideType = RideTypeEnum.Standard,
                RideStatus = RideStatusEnum.Completed,
                Fare = 100,
                Passenger = new GetUserResponseDTO
                {
                    UserId = Guid.NewGuid(),
                    FirstName = "John Doe",
                    Email = "john.doe@example.com",
                    Phone = "+12 345 678 910",
                    Roles = [passengerRole]
                },
                PickupLocation = new GetLocationResponseDTO
                {
                    LocationId = Guid.NewGuid(),
                    Address = "123 Main St",
                    Latitude = 45.0,
                    Longitude = -93.0
                },
                Destination = new GetLocationResponseDTO
                {
                    LocationId = Guid.NewGuid(),
                    Address = "456 Elm St",
                    Latitude = 45.5,
                    Longitude = -93.5
                },
            }
        };

        _mockPaymentRepository.Setup(r => r.GetByIdAsync(paymentId, It.IsAny<List<Expression<Func<Payment, object>>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(payment);
        _mockMapper.Setup(m => m.Map<GetPaymentResponseDTO>(payment)).Returns(responseDto);

        var query = new GetPaymentByIdQuery { PaymentId = paymentId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(responseDto.PaymentId, result.PaymentId);
        Assert.Equal(responseDto.Amount, result.Amount);
        _mockPaymentRepository.Verify(r => r.GetByIdAsync(paymentId, It.IsAny<List<Expression<Func<Payment, object>>>>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_When_Payment_Is_Not_Found()
    {
        // Arrange
        var paymentId = Guid.NewGuid();
        _mockPaymentRepository.Setup(r => r.GetByIdAsync(paymentId, It.IsAny<List<Expression<Func<Payment, object>>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(null as Payment);

        var query = new GetPaymentByIdQuery { PaymentId = paymentId };

        // Act & Assert
        await Assert.ThrowsAsync<BaseException>(() => _handler.Handle(query, CancellationToken.None));
    }
}
