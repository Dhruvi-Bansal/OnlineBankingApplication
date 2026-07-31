using OnlineBankingApplication.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OnlineBankingApplication.Repositories
{
    public interface ITransactionRepo
    {

        public Task<bool> TransferMoney(
            string userId,
            string receiverAccountNumber,
            decimal amount,
            string? description);

        Task<List<Transaction>> GetTransactions(string userId);

        Task<Transaction?> GetTransaction(long id);

        string GenerateReferenceNo();
        Task<int?> GetCurrentAccountId(string userId);
    }
}