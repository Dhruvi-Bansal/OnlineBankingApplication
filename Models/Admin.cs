using System;
using System.Collections.Generic;

namespace OnlineBankingApplication.Models;

public partial class Admin
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public string? ProductType { get; set; }

    public decimal? InterestRate { get; set; }

    public decimal? MinimumBalance { get; set; }

    public string? Description { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();
}
