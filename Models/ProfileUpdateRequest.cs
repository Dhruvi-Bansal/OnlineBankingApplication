using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineBankingApplication.Models
{
 
    public class ProfileUpdateRequest
    {
        public int RequestId { get; set; }

        public int CustomerId { get; set; }


        [StringLength(15)]
        public string? NewPhone { get; set; }


        [StringLength(200)]
        public string? NewAddress { get; set; }


        public DateTime RequestDate { get; set; }

        public string Status { get; set; } = "Pending";

        public DateTime? ApprovedDate { get; set; }

        public string? ApprovedBy { get; set; }

        public virtual Customer Customer { get; set; } = null!;
    }
}