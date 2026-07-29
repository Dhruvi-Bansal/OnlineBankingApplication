using OnlineBankingApplication.Models;

namespace OnlineBankingApplication.Repositories
{
    public interface IProfileUpdateRepo
    {
        Task<List<ProfileUpdateRequest>> GetPendingRequests();

        Task<ProfileUpdateRequest?> GetById(int id);

        Task Approve(int id);

        Task Reject(int id);

        Task Save();
    }
}