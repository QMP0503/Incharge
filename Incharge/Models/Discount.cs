﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Incharge.Models;

public partial class Discount
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public double DiscountValue { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    [AllowedValues(typeof(string), new string[]{"Weekly", "Monthly", "Yearly"})]
    public string? Recurance { get; set; }
    public virtual ICollection<Sale>? Sales { get; set; } //discount applied to salse
}
