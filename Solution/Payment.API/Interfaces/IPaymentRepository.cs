namespace Payment.API.Interfaces;

public interface IPaymentRepository
{
    Task<bool> SaveCashOutAsync(CashOutEntity entity);
    Task<CashOutEntity?> GetCashOutAsync(Guid transactionId);
}