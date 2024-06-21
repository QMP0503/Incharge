using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Incharge.Models;

public partial class Discount
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public decimal Discount1 { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }
    [AllowedValues(typeof(string), new string[]{"Weekly", "Monthly", "Yearly"})]
    public string? Recurance { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
