using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Incharge.Models;

public partial class BusinessReport
{
    public int Id { get; set; }
    public string Uuid { get; set; } = new Guid().ToString();
    public DateTime Date { get; set; }

    public double? Revenue { get; set; }

    public double? Cost { get; set; }

    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
