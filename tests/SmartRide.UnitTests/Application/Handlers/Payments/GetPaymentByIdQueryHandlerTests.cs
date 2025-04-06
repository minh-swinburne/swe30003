using AutoMapper;
using Moq;
using SmartRide.Application.DTOs.Lookup;
using SmartRide.Application.DTOs.Payments;
using SmartRide.Application.Handlers.Payments;
using SmartRide.Application.Queries.Payments;
using SmartRide.Common.Exceptions;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Enums;
using SmartRide.Domain.Interfaces;

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
        var responseDto = new GetPaymentResponseDTO
        {
            PaymentId = paymentId,
            RideId = payment.RideId,
            Amount = payment.Amount,
            Status = payment.Status,
            PaymentMethod = new PaymentMethodDTO
            {
                PaymentMethodId = PaymentMethodEnum.Cash,
                Name = "Cash"
            }
        };

        _mockPaymentRepository.Setup(r => r.GetByIdAsync(paymentId, It.IsAny<CancellationToken>())).ReturnsAsync(payment);
        _mockMapper.Setup(m => m.Map<GetPaymentResponseDTO>(payment)).Returns(responseDto);

        var query = new GetPaymentByIdQuery { PaymentId = paymentId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(responseDto.PaymentId, result.PaymentId);
        Assert.Equal(responseDto.Amount, result.Amount);
        _mockPaymentRepository.Verify(r => r.GetByIdAsync(paymentId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_When_Payment_Is_Not_Found()
    {
        // Arrange
        var paymentId = Guid.NewGuid();
        _mockPaymentRepository.Setup(r => r.GetByIdAsync(paymentId, It.IsAny<CancellationToken>())).ReturnsAsync(null as Payment);

        var query = new GetPaymentByIdQuery { PaymentId = paymentId };

        // Act & Assert
        await Assert.ThrowsAsync<BaseException>(() => _handler.Handle(query, CancellationToken.None));
    }
}
