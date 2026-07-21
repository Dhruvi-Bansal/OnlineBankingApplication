using System;
using System.Collections.Generic;

namespace OnlineBankingApplication.Models;

public partial class BillPayment
{
    public int PaymentId { get; set; }

    public int BillId { get; set; }

    public int AccountId { get; set; }

    public decimal Amount { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string? Status { get; set; }

    public long? TransactionId { get; set; }

    public virtual BankAccount Account { get; set; } = null!;

    public virtual UtilityBill Bill { get; set; } = null!;

    public virtual Transaction? Transaction { get; set; }
}
