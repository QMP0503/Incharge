using System;
using System.Collections.Generic;

namespace Incharge.Models;

public partial class Paymentrecord
{
    public int Id { get; set; }

    public bool? Paymentstatus { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();
}
