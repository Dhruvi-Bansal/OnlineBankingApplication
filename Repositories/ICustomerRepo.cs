using OnlineBankingApplication.Models;

namespace OnlineBankingApplication.Repositories
{
    public interface ICustomerRepo
    {
        Task AddCustomerAsync(Customer customer);

        Task<Customer?> GetCustomerByEmailAsync(string email);

        Task<Customer?> GetCustomerByUserIdAsync(string userId);

        Task<IEnumerable<Customer>> GetPendingCustomersAsync();

        Task ApproveCustomerAsync(int customerId);

        Task RejectCustomerAsync(int customerId);
        Task<Customer?> GetCustomerByIdAsync(int id);


        Task SaveAsync();
    }
}