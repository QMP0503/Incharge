using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Incharge.Models
{
    public class User : IdentityUser
    {
        [MaxLength(100)]
        [Required]
        public string FirstName { get; set; }
        [MaxLength(100)]
        [Required]
        public string LastName { get; set; }
    }
}
