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
    [DataType(DataType.Date)]
    public DateTime PurchaseDate { get; set; }

    [DataType(DataType.Date)]
    public DateTime MaintanceDate { get; set; }
    [AllowedValues(typeof(string), new string[] { "Available", "Unavailable", "Reserved" })]

    public string Status { get; set; } = null!;

    //equipment picture
    public string ? Image { get; set; }

    //store booking for many gym classses... Do I even need an equipment tracker system?
    public virtual ICollection<Gymclass>? GymClasses { get; set; }
}
