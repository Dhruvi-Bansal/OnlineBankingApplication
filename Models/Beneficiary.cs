using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineBankingApplication.Models;

public partial class Beneficiary
{
    public int BeneficiaryId { get; set; }

    public int CustomerId { get; set; }

    [Required]

    [StringLength(50)]
    public string BeneficiaryName { get; set; } = null!;


    [Required]

    [StringLength(18, MinimumLength = 9)]

    [RegularExpression(@"^[0-9]+$",
ErrorMessage = "Invalid Account Number")]
    public string AccountNumber { get; set; } = null!;

    [RegularExpression(@"^[A-Z]{4}0[A-Z0-9]{6}$",
    ErrorMessage = "Invalid IFSC Code.")]

    public string? Ifsccode { get; set; }

    [StringLength(30)]
    public string? NickName { get; set; }

    public DateTime? AddedDate { get; set; }

    public virtual Customer Customer { get; set; } = null!;
}
