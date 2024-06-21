using System;
using System.Collections.Generic;

namespace Incharge.Models;

public partial class Equipment
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? GymClassId { get; set; }

    public string? Description { get; set; }

    public DateTime? PurchaseDate { get; set; }

    public DateTime? MaintanceDate { get; set; }

    public string Satus { get; set; } = null!;

    public virtual Gymclass? GymClass { get; set; }
}
