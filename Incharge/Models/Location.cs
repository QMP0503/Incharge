using System;
using System.Collections.Generic;

namespace Incharge.Models;

public partial class Location
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? Capacity { get; set; }

    public string? Description { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<Gymclass> Gymclasses { get; set; } = new List<Gymclass>();
}
