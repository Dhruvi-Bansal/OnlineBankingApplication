using System.ComponentModel.DataAnnotations;

namespace OnlineBankingApplication.ViewModels
{
    public class RegisterVM
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        [RegularExpression(@"^[6-9]\d{9}$",
            ErrorMessage = "Enter a valid 10-digit mobile number.")]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^\d{12}$",
            ErrorMessage = "Aadhaar must be 12 digits.")]
        public string AadhaarNumber { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z]{5}[0-9]{4}[A-Z]{1}$",
            ErrorMessage = "Invalid PAN format.")]
        public string Pannumber { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        [RegularExpression(@"^\d{6}$")]
        public string Pincode { get; set; }

        [Required]
        public string AccountType { get; set; }

        [Required]
        public string Branch { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}