﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Incharge.Models;

public partial class Paymentrecord
{
    public int Id { get; set; }
    public string Uuid { get; set; }
    [AllowedValues(typeof(string), new string[] { "Paided, Pending, Overdue" })]
    public bool? Paymentstatus { get; set; }
    public string? Description { get; set; }
    public int? ClientId { get; set; }
    public virtual Client Clients { get; set; } //each client will have a personal payment record
    public virtual ICollection<Sale> ClientSales { get; set; } //list of pruchases clients made
}
