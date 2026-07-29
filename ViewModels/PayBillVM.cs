using System.ComponentModel.DataAnnotations;

namespace OnlineBankingApplication.ViewModels
{
    public class PayBillVM
    {
        [Required]
        [Display(Name = "Account Number")]
        public string AccountNumber { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Bill Type")]
        public string BillType { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Provider Name")]
        public string ProviderName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Customer Number")]
        public string CustomerNumber { get; set; } = string.Empty;

        [Required]
        [Range(1, 1000000)]
        public decimal Amount { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Due Date")]
        public DateOnly DueDate { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string Password { get; set; } = string.Empty;
    }
}