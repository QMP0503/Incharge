using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Incharge.Models;

public partial class Gymclass
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime Date { get; set; } //start

    public DateTime EndTime { get; set; } //end

    public string? Description { get; set; }

    [AllowedValues("Active", "Cancelled", "Completed")]
    public string Status { get; set; }

    public int? LocationId { get; set; }

    public int EmployeeId { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual ICollection<Equipment> Equipment { get; set; } = new List<Equipment>();

    public virtual Location? Location { get; set; }

    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();
}
