using System;
using System.Collections.Generic;

namespace Incharge.Models;

public partial class Gymclass
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime Date { get; set; }

    public string? Description { get; set; }

    public int? LocationId { get; set; }

    public byte[]? EmployeeId { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual ICollection<Equipment> Equipment { get; set; } = new List<Equipment>();

    public virtual Location? Location { get; set; }

    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();
}
