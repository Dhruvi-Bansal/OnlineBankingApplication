using System.ComponentModel.DataAnnotations;

namespace OnlineBankingApplication.ViewModels
{
    public class TransferMoneyVM
    {
        [Required(ErrorMessage = "Receiver account number is required")]
        public string ReceiverAccountNumber { get; set; }


        [Required(ErrorMessage = "Amount is required")]
        [Range(1, double.MaxValue,
            ErrorMessage = "Amount must be greater than zero")]
        public decimal Amount { get; set; }


        public string? Description { get; set; }
    }
}