using Microsoft.AspNetCore.Mvc;
using SmartRide.API.Controllers.Base;
using SmartRide.Application.DTOs.Payments;
using SmartRide.Application.Interfaces;

namespace SmartRide.API.Controllers.V1
{
    [Area("v1")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PaymentController : BaseController
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("execute")]
        public async Task<IActionResult> ExecutePayment([FromBody] ExecutePaymentRequestDTO request)
        {
            if (request.Amount <= 0 || string.IsNullOrWhiteSpace(request.Currency))
            {
                return BadRequest("Invalid payment details.");
            }

            var result = await _paymentService.ExecutePaymentAsync(request);

            if (result.Success)
            {
                return Respond(new { Message = result.Message });
            }
            else
            {
                return StatusCode(500, result.Message);
            }
        }
    }
}
