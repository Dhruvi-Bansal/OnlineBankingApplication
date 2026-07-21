using System;
using System.Collections.Generic;

namespace OnlineBankingApplication.Models;

public partial class Beneficiary
{
    public int BeneficiaryId { get; set; }

    public int CustomerId { get; set; }

    public string BeneficiaryName { get; set; } = null!;

    public string AccountNumber { get; set; } = null!;

    public string? Ifsccode { get; set; }

    public string? NickName { get; set; }

    public DateTime? AddedDate { get; set; }

    public virtual Customer Customer { get; set; } = null!;
}
