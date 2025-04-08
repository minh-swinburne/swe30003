using Microsoft.AspNetCore.Mvc;
using SmartRide.API.Controllers.Attributes;
using SmartRide.Application.DTOs.Payments;
using SmartRide.Application.Interfaces;

namespace SmartRide.API.Controllers.V1;

[Area("v1")]
[Pluralize]
public class PaymentController(IPaymentService paymentService) : BaseController
{
    private readonly IPaymentService _paymentService = paymentService;

    // GET: api/v1/payment
    [HttpGet]
    public async Task<IActionResult> GetAllPayments([FromQuery] ListPaymentRequestDTO request)
    {
        var result = await _paymentService.ListPaymentsAsync(request);
        return Respond(result);
    }

    // GET api/v1/payment/<paymentId>
    [HttpGet("{paymentId}")]
    public async Task<IActionResult> GetPaymentById([FromRoute] Guid paymentId)
    {
        var result = await _paymentService.GetPaymentByIdAsync(new GetPaymentByIdRequestDTO { PaymentId = paymentId });
        return Respond(result);
    }

    // PUT api/v1/payment/<paymentId>
    [HttpPut("{paymentId}")]
    public async Task<IActionResult> UpdatePayment([FromRoute] Guid paymentId, [FromBody] UpdatePaymentRequestDTO request)
    {
        request.PaymentId = paymentId;
        var result = await _paymentService.UpdatePaymentAsync(request);
        return Respond(result);
    }
}
