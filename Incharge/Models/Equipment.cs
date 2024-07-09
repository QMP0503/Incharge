using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Incharge.Models;

public partial class Equipment
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? GymClassId { get; set; }

    public string? Description { get; set; }

    public DateTime? PurchaseDate { get; set; }

    public DateTime? MaintanceDate { get; set; }
    [AllowedValues(typeof(string), new string[] { "Available", "Unavailable", "Reserved", "Under Maintance" })]

    public string Status { get; set; } = null!;

    //equipment picture
    public string ? Image { get; set; }
    public virtual Gymclass? GymClass { get; set; }
}
