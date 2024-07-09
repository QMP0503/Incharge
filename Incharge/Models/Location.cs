using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Incharge.Models;

public partial class Location //add image column to help with search
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? Capacity { get; set; }

    public string? Description { get; set; }
    [AllowedValues(typeof(string), new string[] { "Available", "Unavailable", "Under Maintance"})]
    public string? Status { get; set; }
    //Photo of location
    public string? Image { get; set; }
    public virtual ICollection<Gymclass> Gymclasses { get; set; } = new List<Gymclass>();
}
