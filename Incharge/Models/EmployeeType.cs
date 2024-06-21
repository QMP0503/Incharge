using System;
using System.Collections.Generic;

namespace Incharge.Models;

public partial class EmployeeType
{
    public int Id { get; set; }

    public string? Type { get; set; }

    public double? Salary { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
