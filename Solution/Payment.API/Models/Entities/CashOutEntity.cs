namespace Payment.API.Models.Entities;

public class CashOutEntity
{
    public Guid TransactionId { get; set; }
    public BankAccount SourceAccount { get; set; } = new();
    public BankAccount DestinationAccount { get; set; } = new();
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; } = "Pending";
}