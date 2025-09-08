namespace Payment.API.Services;

public class PaymentService(IPaymentRepository paymentRepository) : IPaymentService
{
    public async Task ProcessCashOutAsync(CashOutRequest request)
    {
        var entity = new CashOutEntity
        {
            TransactionId = request.TransactionId,
            SourceAccount = request.SourceAccount,
            DestinationAccount = request.DestinationAccount,
            Amount = request.Amount,
            PaymentDate = request.PaymentDate,
            CreatedAt = DateTime.UtcNow,
            Status = "Pending"
        };

        await paymentRepository.SaveCashOutAsync(entity);
    }

    public async Task<CashOutEntity?> GetCashOutAsync(Guid transactionId)
    {
        return await paymentRepository.GetCashOutAsync(transactionId);
    }
}
