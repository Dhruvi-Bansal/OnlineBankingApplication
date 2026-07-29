using OnlineBankingApplication.Models;

namespace OnlineBankingApplication.Repositories
{
    public interface IBankAccountRepo
    {
        Task<BankAccount?> GetAccountByCustomerId(int customerId);
        Task CreateAccountAsync(BankAccount account);

        Task<string> GenerateAccountNumber();

        Task SaveAsync();
    }
}