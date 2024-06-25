using Incharge.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Incharge.ViewModels
{
    public class EquipmentVM
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public int? GymClassId { get; set; }

        public string? Description { get; set; }

        public DateTime? PurchaseDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? MainationDate { get; set; }
        public string Satus { get; set; } = null!;

        public Gymclass? GymClass { get; set; }
    }
}

