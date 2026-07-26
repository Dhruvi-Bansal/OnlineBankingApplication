using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineBankingApplication.DAL;
using OnlineBankingApplication.Models;

namespace OnlineBankingApplication.Repositories
{
    public class TransactionRepo : ITransactionRepo
    {
        private readonly OnlineBankingDbContext _context;

        public TransactionRepo(OnlineBankingDbContext context)
        {
            _context = context;
        }

        //----------------------------------------------------
        // Load Beneficiaries
        //----------------------------------------------------

        public async Task<IEnumerable<SelectListItem>> GetBeneficiaries(string userId)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (customer == null)
                return new List<SelectListItem>();

            return await _context.Beneficiaries
                .Where(x => x.CustomerId == customer.CustomerId)
                .Select(x => new SelectListItem
                {
                    Value = x.BeneficiaryId.ToString(),
                    Text = x.BeneficiaryName + " - " + x.AccountNumber
                })
                .ToListAsync();
        }

        //----------------------------------------------------
        // Generate Transaction Reference
        //----------------------------------------------------

        public string GenerateReferenceNo()
        {
            return "TXN"
                   + DateTime.Now.ToString("yyyyMMddHHmmss")
                   + Random.Shared.Next(1000, 9999);
        }

        //----------------------------------------------------
        // Transfer Money
        //----------------------------------------------------

        public async Task<bool> TransferMoney(
            string userId,
            int beneficiaryId,
            decimal amount,
            string? description)
        {
            using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                //------------------------------------------------

                var customer = await _context.Customers
                    .FirstOrDefaultAsync(x => x.UserId == userId);

                if (customer == null)
                    return false;

                //------------------------------------------------

                var senderAccount =
                    await _context.BankAccounts
                    .FirstOrDefaultAsync(x =>
                        x.CustomerId == customer.CustomerId &&
                        x.Status == "Active");

                if (senderAccount == null)
                    return false;

                //------------------------------------------------

                var beneficiary =
                    await _context.Beneficiaries
                    .FirstOrDefaultAsync(x =>
                        x.BeneficiaryId == beneficiaryId);

                if (beneficiary == null)
                    return false;

                //------------------------------------------------

                var receiverAccount =
                    await _context.BankAccounts
                    .FirstOrDefaultAsync(x =>
                        x.AccountNumber == beneficiary.AccountNumber &&
                        x.Status == "Active");

                if (receiverAccount == null)
                    return false;

                //------------------------------------------------

                if (senderAccount.AccountId == receiverAccount.AccountId)
                    return false;

                //------------------------------------------------

                if (amount <= 0)
                    return false;

                //------------------------------------------------

                if (senderAccount.Balance < amount)
                    return false;

                //------------------------------------------------
                // Debit Sender
                //------------------------------------------------

                senderAccount.Balance -= amount;

                //------------------------------------------------
                // Credit Receiver
                //------------------------------------------------

                receiverAccount.Balance += amount;

                //------------------------------------------------
                // Transaction Record
                //------------------------------------------------

                Transaction txn = new Transaction
                {
                    TransactionReference = GenerateReferenceNo(),

                    SenderAccountId = senderAccount.AccountId,

                    ReceiverAccountId = receiverAccount.AccountId,

                    Amount = amount,

                    TransactionType = "Fund Transfer",

                    Description = description,

                    Status = "Success",

                    TransactionDate = DateTime.Now
                };

                _context.Transactions.Add(txn);

                //------------------------------------------------

                _context.BankAccounts.Update(senderAccount);

                _context.BankAccounts.Update(receiverAccount);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();

                return false;
            }
        }

        //----------------------------------------------------
        // Transaction History
        //----------------------------------------------------

        public async Task<List<Transaction>> GetTransactions(string userId)
        {
            var customer =
                await _context.Customers
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (customer == null)
                return new List<Transaction>();

            var account =
                await _context.BankAccounts
                .FirstOrDefaultAsync(x =>
                    x.CustomerId == customer.CustomerId);

            if (account == null)
                return new List<Transaction>();

            return await _context.Transactions
                .Include(x => x.SenderAccount)
                .Include(x => x.ReceiverAccount)
                .Where(x =>
                    x.SenderAccountId == account.AccountId ||
                    x.ReceiverAccountId == account.AccountId)
                .OrderByDescending(x => x.TransactionDate)
                .ToListAsync();
        }

        //----------------------------------------------------
        // Single Transaction
        //----------------------------------------------------

        public async Task<Transaction?> GetTransaction(long id)
        {
            return await _context.Transactions
                .Include(x => x.SenderAccount)
                .Include(x => x.ReceiverAccount)
                .FirstOrDefaultAsync(x =>
                    x.TransactionId == id);
        }
        
    }
}