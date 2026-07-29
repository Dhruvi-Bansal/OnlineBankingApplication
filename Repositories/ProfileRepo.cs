using Microsoft.EntityFrameworkCore;
using OnlineBankingApplication.DAL;
using OnlineBankingApplication.Models;
using OnlineBankingApplication.ViewModels;

namespace OnlineBankingApplication.Repositories
{
    // ==========================================================
    // PROFILE FEATURE
    // Repository for Customer Profile
    // ==========================================================
    public class ProfileRepo : IProfileRepo
    {
        private readonly OnlineBankingDbContext _context;

        public ProfileRepo(OnlineBankingDbContext context)
        {
            _context = context;
        }

        // ==========================================================
        // CUSTOMER SIDE
        // ==========================================================

        // NEW
        // Load customer profile along with any pending request
        public async Task<ProfileVM?> GetProfileAsync(string email)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Email == email);

            if (customer == null)
                return null;

            // ------------------------------------------------------
            // Get latest pending request (if any)
            // ------------------------------------------------------

            var request = await _context.ProfileUpdateRequests
                .Where(x => x.CustomerId == customer.CustomerId &&
                            x.Status == "Pending")
                .OrderByDescending(x => x.RequestDate)
                .FirstOrDefaultAsync();

            return new ProfileVM
            {
                CustomerId = customer.CustomerId,

                FirstName = customer.FirstName,

                LastName = customer.LastName,

                Dob = customer.Dob,

                Gender = customer.Gender,

                Email = customer.Email,

                Phone = customer.Phone,

                Address = customer.Address,

                AadhaarNumber = customer.AadhaarNumber,

                Pannumber = customer.Pannumber,

                AccountType = customer.AccountType,

                Branch = customer.Branch,

                // ---------------------------------------------
                // PROFILE FEATURE
                // ---------------------------------------------

                HasPendingRequest = request != null,

                PendingPhone = request?.NewPhone,

                PendingAddress = request?.NewAddress,

                RequestStatus = request?.Status,

                RequestDate = request?.RequestDate
            };
        }

        // ==========================================================

        // NEW
        // Check whether customer already has pending request

        public async Task<bool> HasPendingRequestAsync(int customerId)
        {
            return await _context.ProfileUpdateRequests.AnyAsync(x =>
                x.CustomerId == customerId &&
                x.Status == "Pending");
        }

        // ==========================================================

        // NEW
        // Get pending request

        public async Task<ProfileUpdateRequest?> GetPendingRequestAsync(int customerId)
        {
            return await _context.ProfileUpdateRequests
                .Where(x => x.CustomerId == customerId &&
                            x.Status == "Pending")
                .OrderByDescending(x => x.RequestDate)
                .FirstOrDefaultAsync();
        }

        // ==========================================================

        // NEW
        // Save new profile update request

        public async Task SubmitRequestAsync(ProfileUpdateRequest request)
        {
            await _context.ProfileUpdateRequests.AddAsync(request);
        }

        // ==========================================================
        // ADMIN SIDE
        // ==========================================================

        // NEW
        // Get all pending profile update requests

        public async Task<List<ProfileUpdateRequest>> GetAllPendingRequestsAsync()
        {
            return await _context.ProfileUpdateRequests
                .Include(x => x.Customer)
                .Where(x => x.Status == "Pending")
                .OrderBy(x => x.RequestDate)
                .ToListAsync();
        }

        // ==========================================================

        // NEW
        // Get request by Id

        public async Task<ProfileUpdateRequest?> GetRequestByIdAsync(int requestId)
        {
            return await _context.ProfileUpdateRequests
                .Include(x => x.Customer)
                .FirstOrDefaultAsync(x => x.RequestId == requestId);
        }

        // ==========================================================

        // NEW
        // Approve profile update request

        public async Task ApproveRequestAsync(int requestId,
                                              string approvedBy)
        {
            var request = await GetRequestByIdAsync(requestId);

            if (request == null)
                return;

            // -----------------------------------------
            // Update customer table
            // -----------------------------------------

            request.Customer.Phone = request.NewPhone;

            request.Customer.Address = request.NewAddress;

            // -----------------------------------------
            // Update request table
            // -----------------------------------------

            request.Status = "Approved";

            request.ApprovedDate = DateTime.Now;

            request.ApprovedBy = approvedBy;
        }

        // ==========================================================

        // NEW
        // Reject request

        public async Task RejectRequestAsync(int requestId,
                                             string approvedBy)
        {
            var request = await GetRequestByIdAsync(requestId);

            if (request == null)
                return;

            request.Status = "Rejected";

            request.ApprovedDate = DateTime.Now;

            request.ApprovedBy = approvedBy;
        }

        // ==========================================================

        // NEW
        // Save changes

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}