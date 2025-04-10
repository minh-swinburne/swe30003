using AutoMapper;
using Moq;
using SmartRide.Application.DTOs.Lookup;
using SmartRide.Application.DTOs.Payments;
using SmartRide.Application.Handlers.Payments;
using SmartRide.Application.Queries.Payments;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Enums;
using SmartRide.Domain.Interfaces;
using System.Linq.Expressions;

namespace SmartRide.UnitTests.Application.Handlers.Payments;

public class ListPaymentQueryHandlerTests
{
    private readonly Mock<IRepository<Payment>> _mockPaymentRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly ListPaymentQueryHandler _handler;

    public ListPaymentQueryHandlerTests()
    {
        _mockPaymentRepository = new Mock<IRepository<Payment>>();
        _mockMapper = new Mock<IMapper>();
        _handler = new ListPaymentQueryHandler(_mockPaymentRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_Payments_With_Correct_Count_And_Ids()
    {
        // Arrange
        var payments = new List<Payment>
        {
            new() { Id = Guid.NewGuid(), RideId = Guid.NewGuid(), Amount = 100, Status = PaymentStatusEnum.Completed },
            new() { Id = Guid.NewGuid(), RideId = Guid.NewGuid(), Amount = 200, Status = PaymentStatusEnum.Pending }
        };
        var responseDtos = payments.Select(p => new ListPaymentResponseDTO
        {
            PaymentId = p.Id,
            RideId = p.RideId,
            Amount = p.Amount,
            Status = p.Status,
            PaymentMethod = new PaymentMethodDTO
            {
                PaymentMethodId = p.PaymentMethodId,
                Name = "Cash"
            }
        }).ToList();

        _mockPaymentRepository.Setup(r => r.GetWithFilterAsync(
            It.IsAny<Expression<Func<Payment, bool>>>(),
            It.IsAny<Expression<Func<Payment, ListPaymentResponseDTO>>>(),
            It.IsAny<Expression<Func<Payment, object>>>(),
            It.IsAny<bool>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<List<Expression<Func<Payment, object>>>>(),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(payments);

        _mockMapper.Setup(m => m.Map<List<ListPaymentResponseDTO>>(payments)).Returns(responseDtos);

        var query = new ListPaymentQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(payments.First().Id, result.First().PaymentId);
        Assert.Equal(payments.Last().Id, result.Last().PaymentId);
    }

    [Fact]
    public async Task Handle_Should_Return_Empty_List_When_No_Payments_Found()
    {
        // Arrange
        _mockPaymentRepository.Setup(r => r.GetWithFilterAsync(
            It.IsAny<Expression<Func<Payment, bool>>>(),
            It.IsAny<Expression<Func<Payment, ListPaymentResponseDTO>>>(),
            It.IsAny<Expression<Func<Payment, object>>>(),
            It.IsAny<bool>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<List<Expression<Func<Payment, object>>>>(),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync([]);

        var query = new ListPaymentQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }
}
