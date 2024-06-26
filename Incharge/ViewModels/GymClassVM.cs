using Incharge.Models;

namespace Incharge.ViewModels
{
    public class GymClassVM
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public DateTime Date { get; set; }

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


    }
}
