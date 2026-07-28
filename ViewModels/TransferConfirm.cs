using System.ComponentModel.DataAnnotations;

namespace OnlineBankingApplication.ViewModels
{
    public class TransferConfirmVM
    {
        public int BeneficiaryId { get; set; }

        public decimal Amount { get; set; }

        public string? Description { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";
    }
}
