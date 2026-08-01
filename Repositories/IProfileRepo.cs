using OnlineBankingApplication.Models;
using OnlineBankingApplication.ViewModels;

namespace OnlineBankingApplication.Repositories
{
    public interface IProfileRepo
    {
        Task<ProfileVM?> GetProfileAsync(string email);
        Task<bool> HasPendingRequestAsync(int customerId);
        Task<ProfileUpdateRequest?> GetPendingRequestAsync(int customerId);
        Task SubmitRequestAsync(ProfileUpdateRequest request);

        Task<List<ProfileUpdateRequest>> GetAllPendingRequestsAsync();
        Task<ProfileUpdateRequest?> GetRequestByIdAsync(int requestId);
        Task ApproveRequestAsync(int requestId, string approvedBy);
        Task RejectRequestAsync(int requestId, string approvedBy);
        Task SaveAsync();
    }
}