using Incharge.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Incharge.ViewModels
{
    public class EquipmentVM
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public string? Image { get; set; }//picture of equipment

        [DisplayName("Purchase Date")]
        [DataType(DataType.Date)]
        public DateTime PurchaseDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Maintance Date")]
        public DateTime MaintanceDate { get; set; }

        [AllowedValues("Available", "Unavailable", "Reserved", null)]
        public string? Status { get; set; }

        public ICollection<Gymclass>? GymClasses { get; set; }

        //Date retrieved from view
        public List<int>? GymClassId { get; set; }

        [DisplayName("Add Picture")]
        [AllowedExtensions(new string[] { ".jpg", ".png", ".jpeg", ".jfif" })]
        public IFormFile? PictureFile { get; set; }
        public List<string> StatusOptions { get; set; } = new List<string>() { "Available", "Unavailable", "Reserved" };
       
        //error message
        public string? Error { get; set; }
    }
}

