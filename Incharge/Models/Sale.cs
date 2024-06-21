using System;
using System.Collections.Generic;

namespace Incharge.Models;

public partial class Sale
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public int ProductId { get; set; }

    public byte[]? EmployeeId { get; set; }

    public byte[]? ClientId { get; set; }

    public int? BusinessReportId { get; set; }

    public virtual BusinessReport? BusinessReport { get; set; }

    public virtual Client? Client { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual Product Product { get; set; } = null!;
}
