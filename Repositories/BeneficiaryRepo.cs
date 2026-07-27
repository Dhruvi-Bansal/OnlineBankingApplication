using Microsoft.EntityFrameworkCore;
using OnlineBankingApplication.Models;
using OnlineBankingApplication.DAL;

namespace OnlineBankingApplication.Repositories
{
    public class BeneficiaryRepo : IBeneficiaryRepo
    {
        private readonly OnlineBankingDbContext _context;

        public BeneficiaryRepo(OnlineBankingDbContext context)
        {
            _context = context;
        }


        public async Task<string?> ValidateBeneficiary(int customerId,
                                               string accountNumber,
                                               string? ifscCode)
        {
            // Check Account Exists

            var account = await _context.BankAccounts
                .FirstOrDefaultAsync(x => x.AccountNumber == accountNumber);

            if (account == null)
                return "Account Number does not exist.";

            // IFSC Check

            if (!string.Equals(account.IFSCCode,
                               ifscCode,
                               StringComparison.OrdinalIgnoreCase))
            {
                return "Invalid IFSC Code.";
            }

            // Own Account Check

            if (account.CustomerId == customerId)
            {
                return "You cannot add your own account as beneficiary.";
            }

            // Duplicate Check

            bool exists = await _context.Beneficiaries.AnyAsync(x =>
                x.CustomerId == customerId &&
                x.AccountNumber == accountNumber);

            if (exists)
                return "Beneficiary already exists.";

            return null;
        }
        public async Task<List<Beneficiary>> GetBeneficiaries(int customerId)
        {
            return await _context.Beneficiaries
                .Where(x => x.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task<Beneficiary?> GetById(int id)
        {
            return await _context.Beneficiaries
                .FirstOrDefaultAsync(x => x.BeneficiaryId == id);
        }

        public async Task Add(Beneficiary beneficiary)
        {
            await _context.Beneficiaries.AddAsync(beneficiary);
        }

        public Task Update(Beneficiary beneficiary)
        {
            _context.Beneficiaries.Update(beneficiary);
            return Task.CompletedTask;
        }

        public async Task Delete(int id)
        {
            var ben = await GetById(id);

            if (ben != null)
                _context.Beneficiaries.Remove(ben);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}