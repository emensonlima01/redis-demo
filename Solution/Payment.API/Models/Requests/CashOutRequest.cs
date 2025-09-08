namespace Payment.API.Models.Requests;

public class CashOutRequest
{
    public Guid TransactionId { get; set; }
    public BankAccount SourceAccount { get; set; } = new();
    public BankAccount DestinationAccount { get; set; } = new();
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
}

public class BankAccount
{
    public string BankCode { get; set; } = string.Empty; // Código do banco (3 dígitos)
    public string Agency { get; set; } = string.Empty; // Agência (4 dígitos)
    public string AgencyDigit { get; set; } = string.Empty; // Dígito da agência (1 dígito)
    public string AccountNumber { get; set; } = string.Empty; // Número da conta
    public string AccountDigit { get; set; } = string.Empty; // Dígito da conta
    public string AccountType { get; set; } = string.Empty; // CC (Conta Corrente) ou CP (Poupança)
    public string DocumentNumber { get; set; } = string.Empty; // CPF/CNPJ
    public string AccountHolderName { get; set; } = string.Empty; // Nome do titular
}