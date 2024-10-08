﻿
using System.ComponentModel.DataAnnotations;

namespace Incharge.Models;

public partial class Employee
{
    public int Id { get; set; }
    public string? Uuid { get; set; }
    public int RoleId { get; set; }
    public string FirstName { get; set; } = null!;
    public string Address { get; set; } //employee address
    public string LastName { get; set; } = null!;
    public string? ProfilePicture { get; set; } //get cloudinary working for image upload
    public long Phone { get; set; } //cannot display numbers that start with a 0
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    public double? TotalSalary { get; set; }

    public virtual ICollection<Gymclass> Gymclasses { get; set; } = new List<Gymclass>();

    public virtual EmployeeType Role { get; set; } = null!;

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();

    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();
}
