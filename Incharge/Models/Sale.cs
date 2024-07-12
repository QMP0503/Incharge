using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Incharge.Models;

public partial class Sale 
{
    public int Id { get; set; }
    public string Uuid { get; set; } //relationship will still use regular id.
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
    //Membership will be counted by months
    public double TotalPrice { get; set; } //edit the rest of sales

    public int ProductId { get; set; }

    [AllowedValues(typeof(string), new string[] { "Cash", "Credit", "Debit" })]
    public string? PaymentType { get; set; }

    public int EmployeeId { get; set; }

    public int ClientId { get; set; }

    public int? BusinessReportId { get; set; } //send to business report within the month, will be made during the background.

    public virtual BusinessReport? BusinessReport { get; set; }

    public virtual Client? Client { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual Product Product { get; set; } = null!;

    //list of discounts if more than one discount applies
    public virtual ICollection<Discount> Discounts { get; set; } = new List<Discount>();
}
