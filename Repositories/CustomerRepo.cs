using Microsoft.EntityFrameworkCore;
using OnlineBankingApplication.DAL;
using OnlineBankingApplication.Models;

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

        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(x => x.CustomerId == id);
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
    }
}