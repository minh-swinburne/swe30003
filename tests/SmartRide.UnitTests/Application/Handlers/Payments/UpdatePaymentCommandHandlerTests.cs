using AutoMapper;
using Moq;
using SmartRide.Application.Commands.Payments;
using SmartRide.Application.DTOs.Lookup;
using SmartRide.Application.DTOs.Payments;
using SmartRide.Application.Handlers.Payments;
using SmartRide.Common.Exceptions;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Enums;
using SmartRide.Domain.Interfaces;
using System.Linq.Expressions;

namespace SmartRide.UnitTests.Application.Handlers.Payments;

public class UpdatePaymentCommandHandlerTests
{
    private readonly Mock<IRepository<Payment>> _mockPaymentRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly UpdatePaymentCommandHandler _handler;

    public UpdatePaymentCommandHandlerTests()
    {
        _mockPaymentRepository = new Mock<IRepository<Payment>>();
        _mockMapper = new Mock<IMapper>();
        _handler = new UpdatePaymentCommandHandler(_mockPaymentRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_Should_Update_Payment_And_Return_Response()
    {
        // Arrange
        var paymentId = Guid.NewGuid();
        var command = new UpdatePaymentCommand
        {
            PaymentId = paymentId,
            Amount = 150,
            Status = PaymentStatusEnum.Completed
        };
        var payment = new Payment
        {
            Id = paymentId,
            RideId = Guid.NewGuid(),
            Amount = 100,
            Status = PaymentStatusEnum.Pending
        };
        var updatedPayment = new Payment
        {
            Id = paymentId,
            RideId = payment.RideId,
            Amount = command.Amount.Value,
            Status = command.Status.Value
        };
        var response = new UpdatePaymentResponseDTO
        {
            PaymentId = paymentId,
            Amount = command.Amount.Value,
            Status = command.Status.Value,
            PaymentMethod = new PaymentMethodDTO
            {
                PaymentMethodId = PaymentMethodEnum.Cash,
                Name = "Cash"
            }
        };

        _mockPaymentRepository.Setup(r => r.GetByIdAsync(paymentId, It.IsAny<List<Expression<Func<Payment, object>>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(payment);
        _mockPaymentRepository.Setup(r => r.UpdateAsync(It.IsAny<Payment>(), It.IsAny<CancellationToken>())).ReturnsAsync(updatedPayment);
        _mockMapper.Setup(m => m.Map<UpdatePaymentResponseDTO>(updatedPayment)).Returns(response);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(response.PaymentId, result.PaymentId);
        Assert.Equal(response.Amount, result.Amount);
        _mockPaymentRepository.Verify(r => r.UpdateAsync(It.Is<Payment>(p => p.Id == command.PaymentId), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_When_Payment_Not_Found()
    {
        // Arrange
        var command = new UpdatePaymentCommand { PaymentId = Guid.NewGuid() };
        _mockPaymentRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<List<Expression<Func<Payment, object>>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(null as Payment);

        // Act & Assert
        await Assert.ThrowsAsync<BaseException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
