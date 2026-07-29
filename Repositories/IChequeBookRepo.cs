using OnlineBankingApplication.Models;

namespace OnlineBankingApplication.Repositories
{
    public interface IChequeBookRepo
    {
        Task<IEnumerable<ChequeBookRequest>> GetPendingRequestsAsync();

        Task<ChequeBookRequest> GetRequestByIdAsync(int id);

        Task SaveAsync();
    }
}