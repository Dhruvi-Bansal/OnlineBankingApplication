using Microsoft.EntityFrameworkCore;
using OnlineBankingApplication.DAL;
using OnlineBankingApplication.Models;

namespace OnlineBankingApplication.Repositories
{
    public class ChequeBookRepo : IChequeBookRepo
    {
        private readonly OnlineBankingDbContext _context;

        public ChequeBookRepo(OnlineBankingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ChequeBookRequest>> GetPendingRequestsAsync()
        {
            return await _context.ChequeBookRequests
                .Include(x => x.Account)
                .ThenInclude(x => x.Customer)
                .Where(x => x.Status == "Pending")
                .ToListAsync();
        }

        public async Task<ChequeBookRequest> GetRequestByIdAsync(int id)
        {
            return await _context.ChequeBookRequests
                .FirstOrDefaultAsync(x => x.RequestId == id);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}