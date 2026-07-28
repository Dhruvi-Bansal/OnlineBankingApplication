using System.ComponentModel.DataAnnotations;

namespace OnlineBankingApplication.ViewModels
{
    public class ApproveCustomerVM
    {
        public int CustomerId { get; set; }

        public string CustomerName { get; set; } = "";

        [Required]
        [Range(0, 100000000)]
        public decimal InitialDeposit { get; set; }
    }
}