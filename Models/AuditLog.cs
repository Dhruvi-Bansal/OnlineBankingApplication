using System;
using System.Collections.Generic;

namespace OnlineBankingApplication.Models;

public partial class AuditLog
{
    public long AuditId { get; set; }

    public string? UserId { get; set; }

    public string Action { get; set; } = null!;

    public string? EntityName { get; set; }

    public int? EntityId { get; set; }

    public string? Ipaddress { get; set; }

    public string? Description { get; set; }

    public DateTime? TimeStamp { get; set; }
}
