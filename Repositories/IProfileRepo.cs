using OnlineBankingApplication.Models;
using OnlineBankingApplication.ViewModels;

namespace OnlineBankingApplication.Repositories
{
    // ==========================================================
    // PROFILE FEATURE
    // Repository Interface for Customer Profile
    // ==========================================================
    public interface IProfileRepo
    {
        // ==========================================================
        // CUSTOMER SIDE
        // ==========================================================

        // NEW: Get profile details
        Task<ProfileVM?> GetProfileAsync(string email);

        // NEW: Check if customer already has a pending request
        Task<bool> HasPendingRequestAsync(int customerId);

        // NEW: Get pending request details
        Task<ProfileUpdateRequest?> GetPendingRequestAsync(int customerId);

        // NEW: Submit profile update request
        Task SubmitRequestAsync(ProfileUpdateRequest request);

        // ==========================================================
        // ADMIN SIDE
        // ==========================================================

        // NEW: Get all pending requests
        Task<List<ProfileUpdateRequest>> GetAllPendingRequestsAsync();

        // NEW: Get request by Id
        Task<ProfileUpdateRequest?> GetRequestByIdAsync(int requestId);

        // NEW: Approve request
        Task ApproveRequestAsync(int requestId, string approvedBy);

        // NEW: Reject request
        Task RejectRequestAsync(int requestId, string approvedBy);

        // ==========================================================
        // SAVE CHANGES
        // ==========================================================

        Task SaveAsync();
    }
}