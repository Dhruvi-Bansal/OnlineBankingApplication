using OnlineBankingApplication.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OnlineBankingApplication.Repositories
{
    public interface ITransactionRepo
    {
        Task<IEnumerable<SelectListItem>> GetBeneficiaries(string userId);

        Task<bool> TransferMoney(
            string userId,
            int beneficiaryId,
            decimal amount,
            string? description);

        Task<List<Transaction>> GetTransactions(string userId);

        Task<Transaction?> GetTransaction(long id);

        string GenerateReferenceNo();
    }
}