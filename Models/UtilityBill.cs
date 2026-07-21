using System;
using System.Collections.Generic;

namespace OnlineBankingApplication.Models;

public partial class UtilityBill
{
    public int BillId { get; set; }

    public string BillType { get; set; } = null!;

    public string ProviderName { get; set; } = null!;

    public string CustomerNumber { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateOnly? DueDate { get; set; }

    public virtual ICollection<BillPayment> BillPayments { get; set; } = new List<BillPayment>();
}
