namespace Payment.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController(IPaymentService paymentService) : ControllerBase
{
    [HttpPost("cashout")]
    public async Task<ActionResult> CashOut([FromBody] CashOutRequest request)
    {
        await paymentService.ProcessCashOutAsync(request);
        return Accepted();
    }

    [HttpGet("cashout/{transactionId}")]
    public async Task<ActionResult<CashOutEntity>> GetCashOut(Guid transactionId)
    {
        var cashOut = await paymentService.GetCashOutAsync(transactionId);
        
        if (cashOut == null)
            return NotFound();
            
        return Ok(cashOut);
    }
}
