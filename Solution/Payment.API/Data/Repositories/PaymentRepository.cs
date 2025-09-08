namespace Payment.API.Data.Repositories;

public class PaymentRepository(IRedisContext redisContext) : IPaymentRepository
{
    public async Task<bool> SaveCashOutAsync(CashOutEntity entity)
    {
        var key = $"cashout:{entity.TransactionId}";
        return await redisContext.SaveAsync(key, entity);
    }

    public async Task<CashOutEntity?> GetCashOutAsync(Guid transactionId)
    {
        var key = $"cashout:{transactionId}";
        return await redisContext.GetAsync<CashOutEntity>(key);
    }
}