using System;

namespace OnlineBankingApplication.ViewModels
{
    // ==========================================================
    // PROFILE FEATURE
    // ViewModel used for displaying customer profile
    // ==========================================================

    public class ProfileVM
    {
        // ==========================================================
        // CUSTOMER DETAILS
        // ==========================================================

        public int CustomerId { get; set; }

        public string FirstName { get; set; } = "";

        public string LastName { get; set; } = "";

        public DateOnly? Dob { get; set; }

        public string? Gender { get; set; }

        public string? Email { get; set; }

        public string? AadhaarNumber { get; set; }

        public string? Pannumber { get; set; }

        public string? AccountType { get; set; }

        public string? Branch { get; set; }


        public string? Phone { get; set; }

        public string? Address { get; set; }

       
        public bool HasPendingRequest { get; set; }

        public string? PendingPhone { get; set; }

        public string? PendingAddress { get; set; }


        public string? RequestStatus { get; set; }

     
        public DateTime? RequestDate { get; set; }
    }
}