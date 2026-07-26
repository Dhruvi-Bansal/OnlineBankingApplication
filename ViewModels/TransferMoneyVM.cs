using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OnlineBankingApplication.ViewModels
{
    public class TransferMoneyVM
    {
        [Required]
        [Display(Name = "Beneficiary")]
        public int BeneficiaryId { get; set; }

        [Required]
        [Range(1, double.MaxValue)]
        public decimal Amount { get; set; }

        [StringLength(200)]
        public string? Description { get; set; }

        public IEnumerable<SelectListItem>? Beneficiaries { get; set; }
    }
}