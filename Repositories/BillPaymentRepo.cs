using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineBankingApplication.DAL;
using OnlineBankingApplication.Models;
using OnlineBankingApplication.Repositories.Interfaces;
using OnlineBankingApplication.ViewModels;

namespace OnlineBankingApplication.Repositories
{
    public class BillPaymentRepo : IBillPaymentRepo
    {
        private readonly OnlineBankingDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BillPaymentRepo(
            OnlineBankingDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<(bool Success, string Message, long? TransactionId)>
            PayBillAsync(string userId, PayBillVM model)
        {
            using var dbTransaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Verify logged-in user

                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                    return (false, "User not found.", null);

                // Verify password

                bool passwordValid =
                    await _userManager.CheckPasswordAsync(user, model.Password);

                if (!passwordValid)
                    return (false, "Incorrect password.", null);

                // Customer
                var customer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                if (customer == null)
                    return (false, "Customer not found.", null);

                // Account Validation
                var account = await _context.BankAccounts
                .FirstOrDefaultAsync(a => a.AccountNumber == model.AccountNumber);

                if (account == null)
                {
                    return (false, "Account number does not exist.", null);
                }

                if (account.CustomerId != customer.CustomerId)
                {
                    return (false, "This account does not belong to the logged-in customer.", null);
                }

                // Balance Check
                
                if (account.Balance < model.Amount)
                    return (false, "Insufficient balance.", null);
                // Duplicate Bill Payment Check

                var duplicatePayment = await _context.BillPayments
                .Include(x => x.Bill)
                .AnyAsync(x =>
                    x.AccountId == account.AccountId &&
                    x.Bill.ProviderName == model.ProviderName &&
                    x.Bill.CustomerNumber == model.CustomerNumber &&
                    x.Amount == model.Amount &&
                    x.PaymentDate!.Value.Date == DateTime.Today);

                if (duplicatePayment)
                {
                    return (false,
                        "This bill appears to have already been paid today.",
                        null);
                }

                // Create Utility Bill

                var utilityBill = new UtilityBill
                {
                    BillType = model.BillType,
                    ProviderName = model.ProviderName,
                    CustomerNumber = model.CustomerNumber,
                    Amount = model.Amount,
                    DueDate = model.DueDate
                };

                _context.UtilityBills.Add(utilityBill);

                await _context.SaveChangesAsync();

                // Deduct Balance

                account.Balance -= model.Amount;

                // Create Transaction

                var transaction = new Transaction
                {
                    TransactionReference = Guid.NewGuid()
                        .ToString()
                        .Replace("-", "")
                        .Substring(0, 12)
                        .ToUpper(),

                    SenderAccountId = account.AccountId,
                    ReceiverAccountId = null,

                    Amount = model.Amount,

                    TransactionType = "Utility Bill Payment",

                    Description =
                        $"{model.ProviderName} ({model.CustomerNumber})",

                    Status = "Success",

                    TransactionDate = DateTime.Now
                };

                _context.Transactions.Add(transaction);

                await _context.SaveChangesAsync();

                // Add a record to Bill Payment table

                var payment = new BillPayment
                {
                    BillId = utilityBill.BillId,

                    AccountId = account.AccountId,

                    Amount = model.Amount,

                    PaymentDate = DateTime.Now,

                    Status = "Paid",

                    TransactionId = transaction.TransactionId
                };

                _context.BillPayments.Add(payment);

                // Update Balance

                _context.BankAccounts.Update(account);

                await _context.SaveChangesAsync();

                await dbTransaction.CommitAsync();

                return (true,
                    "Bill paid successfully.",
                    transaction.TransactionId);
            }
            catch
            {
                await dbTransaction.RollbackAsync();

                return (false,
                    "Payment failed.",
                    null);
            }
        }

        // History
        public async Task<List<BillPayment>>
            GetPaymentHistoryAsync(string userId)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (customer == null)
                return new List<BillPayment>();

            return await _context.BillPayments

                .Include(x => x.Bill)

                .Include(x => x.Transaction)

                .Include(x => x.Account)

                .Where(x => x.Account.CustomerId == customer.CustomerId)

                .OrderByDescending(x => x.PaymentDate)

                .ToListAsync();
        }

        // Receipt

        public async Task<BillPayment?>
            GetReceiptAsync(long transactionId)
        {
            return await _context.BillPayments

                .Include(x => x.Bill)

                .Include(x => x.Transaction)

                .Include(x => x.Account)

                .ThenInclude(a => a.Customer)

                .FirstOrDefaultAsync(x =>
                    x.TransactionId == transactionId);
        }
    }
}