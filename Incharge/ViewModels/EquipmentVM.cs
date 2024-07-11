using Incharge.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Incharge.ViewModels
{
    public class EquipmentVM
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public string? Image { get; set; }//picture of equipment

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? PurchaseDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? MaintanceDate { get; set; }
        [AllowedValues(typeof(string), new string[] { "Available", "Unavailable", "Reserved", "Under Maintance" })]

        public string Status { get; set; } = null!;

        public Gymclass? GymClass { get; set; }

        //Date retrieved from view
        public int? GymClassId { get; set; }

        public IFormFile PictureFile { get; set; }
       
    }
}

