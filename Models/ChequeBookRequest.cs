using System;
using System.Collections.Generic;

namespace OnlineBankingApplication.Models;

public partial class ChequeBookRequest
{
    public int RequestId { get; set; }

    public int AccountId { get; set; }

    public int NumberOfLeaves { get; set; }

    public DateTime? RequestDate { get; set; }

    public string? Status { get; set; }

    public string? ApprovedBy { get; set; }

    public DateTime? ApprovalDate { get; set; }

    public virtual BankAccount Account { get; set; } = null!;
}
