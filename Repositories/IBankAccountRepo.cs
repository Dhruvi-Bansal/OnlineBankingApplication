using OnlineBankingApplication.Models;

namespace OnlineBankingApplication.Repositories
{
    public interface IBankAccountRepo
    {
        Task CreateAccountAsync(BankAccount account);

        Task<string> GenerateAccountNumber();

        Task SaveAsync();
    }
}