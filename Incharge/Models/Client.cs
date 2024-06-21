using System;
using System.Collections.Generic;

namespace Incharge.Models;

public partial class Client
{
    public byte[] Id { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public int Phone { get; set; }

    public string Email { get; set; } = null!;

    public string? Status { get; set; }

    public int? PaymentRecordId { get; set; }

    public virtual Paymentrecord? PaymentRecord { get; set; }

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<Gymclass> Gymclasses { get; set; } = new List<Gymclass>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
