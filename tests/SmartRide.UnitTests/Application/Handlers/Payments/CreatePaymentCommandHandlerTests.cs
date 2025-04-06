using AutoMapper;
using Moq;
using SmartRide.Application.Commands.Payments;
using SmartRide.Application.DTOs.Lookup;
using SmartRide.Application.DTOs.Payments;
using SmartRide.Application.Handlers.Payments;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Enums;
using SmartRide.Domain.Interfaces;

namespace SmartRide.UnitTests.Application.Handlers.Payments;

public class CreatePaymentCommandHandlerTests
{
    private readonly Mock<IRepository<Payment>> _mockPaymentRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly CreatePaymentCommandHandler _handler;

    public CreatePaymentCommandHandlerTests()
    {
        _mockPaymentRepository = new Mock<IRepository<Payment>>();
        _mockMapper = new Mock<IMapper>();
        _handler = new CreatePaymentCommandHandler(_mockPaymentRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_Should_Create_Payment_And_Return_Response()
    {
        // Arrange
        var command = new CreatePaymentCommand
        {
            RideId = Guid.NewGuid(),
            Amount = 100,
            PaymentMethodId = PaymentMethodEnum.Cash
        };
        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            RideId = command.RideId,
            Amount = command.Amount,
            PaymentMethodId = command.PaymentMethodId
        };
        var response = new CreatePaymentResponseDTO
        {
            PaymentId = payment.Id,
            RideId = payment.RideId,
            Amount = payment.Amount,
            Status = PaymentStatusEnum.Pending,
            PaymentMethod = new PaymentMethodDTO { PaymentMethodId = payment.PaymentMethodId, Name = "Cash" }
        };

        _mockMapper.Setup(m => m.Map<Payment>(command)).Returns(payment);
        _mockPaymentRepository.Setup(r => r.CreateAsync(payment, It.IsAny<CancellationToken>())).ReturnsAsync(payment);
        _mockMapper.Setup(m => m.Map<CreatePaymentResponseDTO>(payment)).Returns(response);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(response.PaymentId, result.PaymentId);
        Assert.Equal(response.Amount, result.Amount);
        _mockPaymentRepository.Verify(r => r.CreateAsync(payment, It.IsAny<CancellationToken>()), Times.Once);
    }
}
