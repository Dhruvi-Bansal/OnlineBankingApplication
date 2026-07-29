using OnlineBankingApplication.Models;
using OnlineBankingApplication.ViewModels;

namespace OnlineBankingApplication.Repositories.Interfaces
{
    public interface IBillPaymentRepo
    {
        Task<(bool Success, string Message, long? TransactionId)>
            PayBillAsync(string userId, PayBillVM model);

        Task<List<BillPayment>> GetPaymentHistoryAsync(string userId);

        Task<BillPayment?> GetReceiptAsync(long transactionId);
    }
}