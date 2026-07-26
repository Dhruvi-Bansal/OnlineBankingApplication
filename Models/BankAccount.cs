using System;
using System.Collections.Generic;

namespace OnlineBankingApplication.Models;

public partial class BankAccount
{
    public int AccountId { get; set; }

    public int CustomerId { get; set; }

    public int ProductId { get; set; }

    public string AccountNumber { get; set; } = null!;

    public string AccountType { get; set; } = null!;

    public decimal Balance { get; set; }

    public string? Ifsccode { get; set; }
    public string IFSCCode { get; internal set; }
    public string? BranchName { get; set; }

    public string? Status { get; set; }

    public DateTime? OpenedDate { get; set; }

    public virtual ICollection<BillPayment> BillPayments { get; set; } = new List<BillPayment>();

    public virtual ICollection<ChequeBookRequest> ChequeBookRequests { get; set; } = new List<ChequeBookRequest>();

    public virtual Customer Customer { get; set; } = null!;

    public virtual Admin Product { get; set; } = null!;

    public virtual ICollection<Transaction> TransactionReceiverAccounts { get; set; } = new List<Transaction>();

    public virtual ICollection<Transaction> TransactionSenderAccounts { get; set; } = new List<Transaction>();
}
