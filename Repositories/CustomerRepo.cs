using Microsoft.EntityFrameworkCore;
using OnlineBankingApplication.Models;
using OnlineBankingApplication.ViewModels;

namespace OnlineBankingApplication.Repositories
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly OnlineBankingDbContext _context;

        public CustomerRepo(OnlineBankingDbContext context)
        {
            _context = context;
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
        }

        public async Task<Customer?> GetCustomerByEmailAsync(string email)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<Customer?> GetCustomerByUserIdAsync(string userId)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<IEnumerable<Customer>> GetPendingCustomersAsync()
        {
            return await _context.Customers
                .Where(x => x.Status == "Pending")
                .ToListAsync();
        }

        public async Task ApproveCustomerAsync(int customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId);

            if (customer != null)
            {
                customer.Status = "Approved";
            }
        }

        public async Task RejectCustomerAsync(int customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId);

            if (customer != null)
            {
                customer.Status = "Rejected";
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }


        public async Task<CustomerDashboardVM> GetDashboard(string email)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(x => x.Email == email);

            if (customer == null)
                return null;

            var account = await _context.BankAccounts
                .FirstOrDefaultAsync(x => x.CustomerId == customer.CustomerId);

            var transactions = new List<Transaction>();

            if (account != null)
            {
                transactions = await _context.Transactions
                .Where(x => x.SenderAccountId == account.AccountId
                  || x.ReceiverAccountId == account.AccountId)
                 .OrderByDescending(x => x.TransactionDate)
                                          .Take(5)
                                          .ToListAsync();
            }

            return new CustomerDashboardVM
            {
                CustomerName = customer.FirstName + " " + customer.LastName,

                Status = customer.Status,

                AccountNumber = account == null ?
                                "Pending Approval" :
                                account.AccountNumber,

                AccountType = account == null ?
                              "Savings Account" :
                              account.AccountType,

                Balance = account == null ?
                          0 :
                          account.Balance,

                RecentTransactions = transactions
            };
        }

      
    }
}