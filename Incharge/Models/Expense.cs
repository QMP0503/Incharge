using System;
using System.Collections.Generic;

namespace Incharge.Models;

public partial class Expense
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public string Name { get; set; } = null!;

    public double Cost { get; set; }

    public string? Description { get; set; }

    public int? BusinessReportId { get; set; }

    public virtual BusinessReport? BusinessReport { get; set; }
}
