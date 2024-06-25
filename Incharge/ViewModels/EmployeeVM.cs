﻿using Incharge.Models;
using System.ComponentModel.DataAnnotations;

namespace Incharge.ViewModels
{
    public class EmployeeVM
    {
        public string? Uuid { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public int Phone { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
        public double? TotalSalary { get; set; }
        public virtual List<int> GymclassesId { get; set; } = new List<int>();
        public int RoleId { get; set; }
        public virtual List<int> SalesId { get; set; } = new List<int>();
        public virtual List<int> ClientId { get; set; } = new List<int>();
    }
}
