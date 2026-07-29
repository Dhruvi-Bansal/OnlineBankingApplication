using Microsoft.EntityFrameworkCore;
using OnlineBankingApplication.DAL;
using OnlineBankingApplication.Models;

namespace OnlineBankingApplication.Repositories
{
    public class BankAccountRepo : IBankAccountRepo
    {
        private readonly OnlineBankingDbContext _context;

        public BankAccountRepo(OnlineBankingDbContext context)
        {
            _context = context;
        }

        public async Task CreateAccountAsync(BankAccount account)
        {
            await _context.BankAccounts.AddAsync(account);
        }

        public async Task<BankAccount?> GetAccountByCustomerId(int customerId)
        {
            return await _context.BankAccounts
                .FirstOrDefaultAsync(x => x.CustomerId == customerId);
        }

        public async Task<string> GenerateAccountNumber()
        {
            Random random = new Random();

            string accountNumber;

            do
            {
                accountNumber =
                    random.Next(100000, 999999).ToString() +
                    random.Next(100000, 999999).ToString();

            } while (await _context.BankAccounts
                .AnyAsync(x => x.AccountNumber == accountNumber));

            return accountNumber;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}