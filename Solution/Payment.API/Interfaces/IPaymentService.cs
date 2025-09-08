namespace Payment.API.Interfaces;

public interface IPaymentService
{
    Task ProcessCashOutAsync(CashOutRequest request);
    Task<CashOutEntity?> GetCashOutAsync(Guid transactionId);
}