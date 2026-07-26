using OnlineBankingApplication.Models;

namespace OnlineBankingApplication.Repositories
{
    public interface IBeneficiaryRepo
    {
        Task<string?> ValidateBeneficiary(int customerId,
                                  string accountNumber,
                                  string? ifscCode);
        Task<List<Beneficiary>> GetBeneficiaries(int customerId);

        Task<Beneficiary?> GetById(int id);

        Task Add(Beneficiary beneficiary);

        Task Update(Beneficiary beneficiary);

        Task Delete(int id);

        Task Save();
    }
}