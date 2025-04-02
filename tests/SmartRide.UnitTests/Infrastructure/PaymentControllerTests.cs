using Moq;
using Microsoft.AspNetCore.Mvc;
using SmartRide.Application.DTOs.Payments;
using SmartRide.Application.Interfaces;
using SmartRide.API.Controllers.V1;
using Xunit;

namespace SmartRide.UnitTests.API.Controllers
{
    public class PaymentControllerTests
    {
        private readonly Mock<IPaymentService> _paymentServiceMock;
        private readonly PaymentController _controller;

        public PaymentControllerTests()
        {
            _paymentServiceMock = new Mock<IPaymentService>();
            _controller = new PaymentController(_paymentServiceMock.Object);
        }

        [Fact]
        public async Task ExecutePayment_ShouldReturnSuccess_WhenPaymentIsProcessed()
        {
            // Arrange
            var paymentRequest = new ExecutePaymentRequestDTO
            {
                Amount = 100,
                Currency = "USD"
            };

            // Mock the ExecutePaymentAsync method to return true (payment is processed successfully)
            _paymentServiceMock
                .Setup(service => service.ExecutePaymentAsync(paymentRequest.Amount, paymentRequest.Currency))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.ExecutePayment(paymentRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value as dynamic;
            Assert.Equal("Payment processed successfully.", response.Message);
        }

        [Fact]
        public async Task ExecutePayment_ShouldReturnBadRequest_WhenPaymentDetailsAreInvalid()
        {
            // Arrange
            var paymentRequest = new ExecutePaymentRequestDTO
            {
                Amount = 0, // Invalid amount
                Currency = "USD"
            };

            // Act
            var result = await _controller.ExecutePayment(paymentRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid payment details.", badRequestResult.Value);
        }

        [Fact]
        public async Task ExecutePayment_ShouldReturnFailure_WhenPaymentFails()
        {
            // Arrange
            var paymentRequest = new ExecutePaymentRequestDTO
            {
                Amount = 100,
                Currency = "USD"
            };

            // Mock the ExecutePaymentAsync method to return false (payment fails)
            _paymentServiceMock
                .Setup(service => service.ExecutePaymentAsync(paymentRequest.Amount, paymentRequest.Currency))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.ExecutePayment(paymentRequest);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("Payment failed.", statusCodeResult.Value);
        }
    }
}
