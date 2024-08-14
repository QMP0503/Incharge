using Incharge.Models;
using System.ComponentModel.DataAnnotations;

namespace Incharge.ViewModels
{
    public class LocationVM
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public int? Capacity { get; set; }

        public string? Image { get; set; } //image for the location

        public string? Description { get; set; }

        [AllowedValues("Available", "Unavailable", "Reserved", "Under Maintance", null)]
        public string? Status { get; set; }

        //object list to display on view
        public virtual ICollection<Gymclass>? Gymclasses { get; set; }
        //data retived from view
        public virtual List<int> GymclassesId { get; set; } = new List<int>();

        //Image Profile
        public IFormFile? ImageFile { get; set; }

        //Error message
        public string? Error { get; set; }
    }
}
