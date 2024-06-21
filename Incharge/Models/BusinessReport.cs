using System;
using System.Collections.Generic;

namespace Incharge.Models;

public partial class BusinessReport
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public double? Revenue { get; set; }

    public double? Cost { get; set; }

    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
