using OnlineBankingApplication.Models;

namespace OnlineBankingApplication.ViewModels
{
    public class CustomerDashboardVM
    {
        public int AccountId { get; set; }

        public string CustomerName { get; set; } = "";

        public string AccountNumber { get; set; } = "";

        public string AccountType { get; set; } = "";

        public decimal Balance { get; set; }

        public string Status { get; set; } = "";

        public List<Transaction> RecentTransactions { get; set; }
            = new();
    }
}