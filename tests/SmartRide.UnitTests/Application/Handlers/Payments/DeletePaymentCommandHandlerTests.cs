using Moq;
using SmartRide.Application.Commands.Payments;
using SmartRide.Application.DTOs.Payments;
using SmartRide.Application.Handlers.Payments;
using SmartRide.Common.Exceptions;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.UnitTests.Application.Handlers.Payments;

public class DeletePaymentCommandHandlerTests
{
    private readonly Mock<IRepository<Payment>> _mockPaymentRepository;
    private readonly DeletePaymentCommandHandler _handler;

    public DeletePaymentCommandHandlerTests()
    {
        _mockPaymentRepository = new Mock<IRepository<Payment>>();
        _handler = new DeletePaymentCommandHandler(_mockPaymentRepository.Object);
    }

    [Fact]
    public async Task Handle_Should_Delete_Payment_And_Return_Response()
    {
        // Arrange
        var paymentId = Guid.NewGuid();
        var payment = new Payment
        {
            Id = paymentId,
            RideId = Guid.NewGuid(),
            Amount = 100
        };
        _mockPaymentRepository.Setup(r => r.GetByIdAsync(paymentId, It.IsAny<List<string>>(), It.IsAny<CancellationToken>())).ReturnsAsync(payment);
        _mockPaymentRepository.Setup(r => r.DeleteAsync(paymentId, It.IsAny<CancellationToken>())).ReturnsAsync(payment);

        var command = new DeletePaymentCommand { PaymentId = paymentId };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.Equal(paymentId, result.PaymentId);
        _mockPaymentRepository.Verify(r => r.DeleteAsync(paymentId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_When_Payment_Not_Found()
    {
        // Arrange
        var command = new DeletePaymentCommand { PaymentId = Guid.NewGuid() };
        _mockPaymentRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<List<string>>(), It.IsAny<CancellationToken>())).ReturnsAsync(null as Payment);

        // Act & Assert
        await Assert.ThrowsAsync<BaseException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
