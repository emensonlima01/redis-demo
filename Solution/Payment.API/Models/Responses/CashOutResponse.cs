namespace Payment.API.Models.Responses;

public class CashOutResponse
{
    public string TransactionId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime ProcessedAt { get; set; }
    public string Message { get; set; } = string.Empty;
}