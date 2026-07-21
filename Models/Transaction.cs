using System;
using System.Collections.Generic;

namespace OnlineBankingApplication.Models;

public partial class Transaction
{
    public long TransactionId { get; set; }

    public string TransactionReference { get; set; } = null!;

    public int? SenderAccountId { get; set; }

    public int? ReceiverAccountId { get; set; }

    public decimal Amount { get; set; }

    public string TransactionType { get; set; } = null!;

    public string? Description { get; set; }

    public string? Status { get; set; }

    public DateTime? TransactionDate { get; set; }

    public virtual ICollection<BillPayment> BillPayments { get; set; } = new List<BillPayment>();

    public virtual BankAccount? ReceiverAccount { get; set; }

    public virtual BankAccount? SenderAccount { get; set; }
}
