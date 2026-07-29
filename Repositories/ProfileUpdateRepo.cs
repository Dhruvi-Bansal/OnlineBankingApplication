using Microsoft.EntityFrameworkCore;
using OnlineBankingApplication.DAL;
using OnlineBankingApplication.Models;

namespace OnlineBankingApplication.Repositories
{
    public class ProfileUpdateRepo : IProfileUpdateRepo
    {
        private readonly OnlineBankingDbContext _context;

        public ProfileUpdateRepo(OnlineBankingDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProfileUpdateRequest>> GetPendingRequests()
        {
            return await _context.ProfileUpdateRequests
                .Include(x => x.Customer)
                .Where(x => x.Status == "Pending")
                .OrderBy(x => x.RequestDate)
                .ToListAsync();
        }

        public async Task<ProfileUpdateRequest?> GetById(int id)
        {
            return await _context.ProfileUpdateRequests
                .Include(x => x.Customer)
                .FirstOrDefaultAsync(x => x.RequestId == id);
        }

        public async Task Approve(int id)
        {
            var request = await GetById(id);

            if (request == null)
                return;
            request.Customer.Address = request.NewAddress;
            request.Customer.Phone = request.NewPhone;

          
            request.Status = "Approved";

          
            request.ApprovedDate = DateTime.Now;

        }


        public async Task Reject(int id)
        {
            var request = await GetById(id);

            if (request == null)
                return;

            request.Status = "Rejected";

           
            request.ApprovedDate = DateTime.Now;

        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}