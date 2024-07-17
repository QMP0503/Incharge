using Incharge.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Incharge.ViewModels
{
    public class GymClassVM
    {
        public int Id { get; set; }
        
        public string Name { get; set; } = null!;

        [AllowedValues("Private", "Group")]
        public string Type { get; set; } = null!;

        [DisplayName("Start")]
        public DateTime Date { get; set; }

        [DisplayName("End")]
        public DateTime EndTime { get; set; } //end


        [AllowedValues("Active","Cancelled","Completed", null)]
        public string? Status { get; set; }
        public string? Description { get; set; }

        //selected from view list
        public int LocationId { get; set; }
        public int EmployeeId { get; set; }
        public List<int>? EquipmentId { get; set; }
        public List<int>? ClientsId { get; set; }


        //object list
        public virtual Employee? Employee { get; set; }

        public virtual ICollection<Equipment> Equipment { get; set; } = new List<Equipment>();

        public virtual Location? Location { get; set; }

        public virtual ICollection<Client> Clients { get; set; } = new List<Client>();


        //LIST FOR SELECTION ONLY
        [DisplayName("Time Slots")]
        [AllowNull]
        public List<TimeSpan>? TimeSlots { get; set; }

        [DisplayName("Instructor Names")]
        public List<Employee>? EmployeeOptions { get; set; }//to track the employee that made the sale

        [DisplayName("Locations")]
        public List<Location>? LocationOptions { get; set; }

        [DisplayName("Equipments Required")]
        public List<Equipment>? EquipmentOptions { get; set; }

        [DisplayName("Clients")]
        public List<Client>? ClientOptions { get; set; }

        [DisplayName("Status")]
        public List<string> StatusOptions { get; set; } = new List<string>() { "Active", "Cancelled", "Completed" };
        
        [DisplayName("Class Type")]
        public List<string> TypeOptions { get; set; } = new List<string>() { "Private", "Group" };
        //Error
        public string? Error{ get; set; }
    }
}
